using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using lindotnet.Classes.Component.Implementation;
using lindotnet.Classes.Helpers;
using lindotnet.Classes.Wrapper.Implementation.Modules;
using lindotnet.Classes.Wrapper.Interfaces;

namespace lindotnet.Classes.Wrapper.Implementation
{
    internal class LinphoneWrapper : ILinphoneWrapper
    {
        #region Fields

        private LinphoneDelegates.LogEventCb logevent_cb;

        private LinphoneDelegates.LinphoneCoreRegistrationStateChangedCb registration_state_changed;

        private LinphoneDelegates.LinphoneCoreCallStateChangedCb call_state_changed;

        private LinphoneDelegates.LinphoneCoreCbsMessageReceivedCb message_received;

        private Thread coreLoop;

        #endregion

        #region Props

        public IntPtr LinphoneCore { get; private set; }

        public IntPtr ProxyCfg { get; private set; }

        public IntPtr AuthInfo { get; private set; }

        public IntPtr TransportConfigPtr { get; private set; }

        public IntPtr VTablePtr { get; private set; }

        public IntPtr NatPolicy { get; private set; }

        public bool IsRunning { get; private set; } = false;

        public string Identity { get; private set; }

        public string ServerHost { get; private set; }

        public LinphoneStructs.LinphoneCoreVTable VTable { get; private set; }

        public LinphoneStructs.LCSipTransports TransportConfig { get; private set; }

        public ConcurrentDictionary<IntPtr, LinphoneCall> Calls { get; private set; }

        public bool LogsEnabled { get; set; }

        public CallParamsBuilder CallParamsBuilder { get; set; }

        #endregion

        #region Log

        public delegate void LogDelegate(string message);

        private event LogDelegate logEventHandler;

        public event LogDelegate LogEvent
        {
            add
            {
                if (logEventHandler == null && LogsEnabled)
                {
                    CoreModule.linphone_core_set_log_level(OrtpLogLevel.DEBUG);
                    if (logevent_cb == null)
                    {
                        logevent_cb = new LinphoneDelegates.LogEventCb(LinphoneLogEvent);
                    }

                    CoreModule.linphone_core_set_log_handler(Marshal.GetFunctionPointerForDelegate(logevent_cb));
                }
                logEventHandler += value;
            }
            remove
            {
                logEventHandler -= value;
                if (logEventHandler == null)
                {
                    CoreModule.linphone_core_set_log_level(OrtpLogLevel.END);
                }
            }
        }

        #endregion

        #region Events

        public event RegistrationStateChangedDelegate RegistrationStateChangedEvent;

        public event CallStateChangedDelegate CallStateChangedEvent;

        public event ErrorDelegate ErrorEvent;

        public event MessageReceivedDelegate MessageReceivedEvent;

        #endregion

        #region Delegates

        public delegate void RegistrationStateChangedDelegate(LinphoneRegistrationState state);

        public delegate void CallStateChangedDelegate(Call call);

        public delegate void ErrorDelegate(Call call, string message);

        public delegate void MessageReceivedDelegate(string from, string message);

        #endregion

#if (DEBUG)
#warning Loader wouldn't work!
        static LinphoneWrapper()
        {
            IntPtr dllPtr = DllLoader.DoLoadLibrary(Constants.LIBNAME);

            var Modules = from t in Assembly.GetExecutingAssembly().GetTypes()
                          where t.IsClass && t.Namespace == "Modules"
                          select t;

            foreach (var module in Modules)
            {
                foreach (MethodInfo info in module.GetMethods(BindingFlags.Public | BindingFlags.Static))
                {
                    if (DllLoader.DoGetProcAddress(dllPtr, info.Name).IsZero())
                    {
                        throw new EntryPointNotFoundException($"Invalid linphone library version: {info.Name} is not found.");
                    }
                }
            }


            DllLoader.DoFreeLibrary(dllPtr);
        }
#endif

        public LinphoneWrapper()
        {
            Calls = new ConcurrentDictionary<IntPtr, LinphoneCall>();

            CoreModule.linphone_core_set_log_level(OrtpLogLevel.END);
        }

        #region Interface Imlementation

        public void CreatePhone(LinphoneConnectionParams connectionParams)
        {
            IsRunning = true;

            registration_state_changed = new LinphoneDelegates.LinphoneCoreRegistrationStateChangedCb(OnRegistrationChanged);
            call_state_changed = new LinphoneDelegates.LinphoneCoreCallStateChangedCb(OnCallStateChanged);
            message_received = new LinphoneDelegates.LinphoneCoreCbsMessageReceivedCb(OnMessageReceived);

            VTable = CreateLinphoneCoreVTable();
            VTablePtr = VTable.ToIntPtr();

#warning Deprecated Now, use factory methods
            LinphoneCore = CoreModule.linphone_core_new(VTablePtr, null, null, IntPtr.Zero);

            coreLoop = new Thread(LinphoneMainLoop);
            coreLoop.IsBackground = false;
            coreLoop.Start();

            TransportConfig = CreateTransportConfig();
            TransportConfigPtr = TransportConfig.ToIntPtr();
            NetworkModule.linphone_core_set_sip_transports(LinphoneCore, TransportConfigPtr);

            GenericModules.linphone_core_set_user_agent(LinphoneCore, connectionParams.Agent, connectionParams.Version);

            if (string.IsNullOrEmpty(connectionParams.AccountAlias))
            {
                Identity = $"sip:{connectionParams.Username}@{connectionParams.Host}";
            }
            else
            {
                Identity = $"\"{connectionParams.AccountAlias}\" sip:{connectionParams.Username}@{connectionParams.Host}";
            }

            ServerHost = $"sip:{connectionParams.Host}:{connectionParams.Port}";

            AuthInfo = GenericModules.linphone_auth_info_new(connectionParams.Username, null, connectionParams.Password, null, null, null);
            GenericModules.linphone_core_add_auth_info(LinphoneCore, AuthInfo);

            NatPolicy = CreateNatPolicy(connectionParams.NatPolicy);

            ProxyCfg = CreateProxyCfg();

            CallParamsBuilder = new CallParamsBuilder(LinphoneCore);
        }

        public void DestroyPhone()
        {
            if (LinphoneCore != null)
            {
                RegistrationStateChangedEvent?.Invoke(LinphoneRegistrationState.LinphoneRegistrationProgress);

                CallModule.linphone_core_terminate_all_calls(LinphoneCore);

                var proxySetDownTask = ExecuteWithDelay(() =>
                {
                    if (ProxieModule.linphone_proxy_config_is_registered(ProxyCfg))
                    {
                        ProxieModule.linphone_proxy_config_edit(ProxyCfg);
                        ProxieModule.linphone_proxy_config_enable_register(ProxyCfg, false);
                        ProxieModule.linphone_proxy_config_done(ProxyCfg);
                    }
                }, Constants.LC_CORE_PROXY_DISABLE_TIMEOUT);

                proxySetDownTask.Wait();

                IsRunning = ProxieModule.linphone_proxy_config_is_registered(ProxyCfg);
            }
        }

        public void LinphoneLogEvent(string domain, OrtpLogLevel lev, string fmt, IntPtr args)
        {
            logEventHandler?.Invoke(DllLoader.ProcessVAlist(fmt, args));
        }

        public void MakeCall(string uri)
        {
            MakeCallAndRecord(uri, null, false);
        }

        public void MakeCallAndRecord(string uri, string filename, bool startRecordInstantly)
        {
            if (LinphoneCore.IsNonZero())
            {
                IntPtr callParams = CallParamsBuilder
                    .BuildAudioParams()
                    .BuildVideoParams()
                    .BuildMediaParams()
                    .Build();

                if (!string.IsNullOrWhiteSpace(filename))
                {
                    CallModule.linphone_call_params_set_record_file(callParams, filename);
                }

                IntPtr call = CallModule.linphone_core_invite_with_params(LinphoneCore, uri, callParams);

                if (call.IsZero())
                {
                    ErrorEvent?.Invoke(null, "Can't call!");
                    return;
                }

                CallModule.linphone_call_ref(call);
                if (startRecordInstantly)
                {
                    GenericModules.linphone_call_start_recording(call);
                }
            }
        }

        public void OnCallStateChanged(IntPtr lc, IntPtr call, LinphoneCallState callState, string message)
        {
            if (LinphoneCore.IsNonZero() && IsRunning)
            {
                var newCallState = CallState.None;
                var newCallType = CallType.None;
                string from, to, recordFile;

                from = to = recordFile = null;
                IntPtr callParams = CallModule.linphone_call_get_params(call);

                bool recordEnable = MarshalingExtensions.TryConvert(CallModule.linphone_call_params_get_record_file(callParams), out recordFile);

                // detecting direction, state and source-destination data by state
                switch (callState)
                {
                    case LinphoneCallState.LinphoneCallIncomingReceived:
                    case LinphoneCallState.LinphoneCallIncomingEarlyMedia:
                        newCallState = CallState.Loading;
                        newCallType = CallType.Incoming;
                        MarshalingExtensions.TryConvert(CallModule.linphone_call_get_remote_address_as_string(call), out from);
                        to = Identity;
                        break;

                    case LinphoneCallState.LinphoneCallConnected:
                    case LinphoneCallState.LinphoneCallResuming:
                    case LinphoneCallState.LinphoneCallStreamsRunning:
                    case LinphoneCallState.LinphoneCallPausedByRemote:
                    case LinphoneCallState.LinphoneCallUpdatedByRemote:
                        newCallState = CallState.Active;
                        break;

                    case LinphoneCallState.LinphoneCallPaused:
                    case LinphoneCallState.LinphoneCallPausing:
                        newCallState = CallState.Hold;
                        break;

                    case LinphoneCallState.LinphoneCallOutgoingInit:
                    case LinphoneCallState.LinphoneCallOutgoingProgress:
                    case LinphoneCallState.LinphoneCallOutgoingRinging:
                    case LinphoneCallState.LinphoneCallOutgoingEarlyMedia:
                        newCallState = CallState.Loading;
                        newCallType = CallType.Outcoming;
                        from = Identity;
                        MarshalingExtensions.TryConvert(CallModule.linphone_call_get_remote_address_as_string(call), out to);
                        break;

                    case LinphoneCallState.LinphoneCallError:
                        newCallState = CallState.Error;
                        break;

                    case LinphoneCallState.LinphoneCallReleased:
                    case LinphoneCallState.LinphoneCallEnd:
                        newCallState = CallState.Completed;
                        if (recordEnable)
                        {
                            GenericModules.linphone_call_stop_recording(call);
                        }
                        break;
                    default:
                        throw new NotImplementedException("Sorry, that feature not implemented!");
                        break;
                }

                // Update references
                IntPtr callref = CallModule.linphone_call_ref(call);
                if (callref.IsNonZero())
                {
                    LinphoneCall existCall = null;
                    if (Calls.TryGetValue(callref, out existCall))
                    {
                        if (existCall.State != newCallState)
                        {
                            existCall.State = newCallState;
                            CallStateChangedEvent?.Invoke(existCall);
                        }

                        if (callState == LinphoneCallState.LinphoneCallReleased)
                        {
                            CallModule.linphone_call_unref(existCall.LinphoneCallPtr);
                            if (Calls.TryRemove(callref, out existCall))
                            {
                                throw new LinphoneException("Call didnt remove from queue!");
                            }
                            return;
                        }
                    }
                    else
                    {
                        existCall = new LinphoneCall()
                        {
                            State = newCallState,
                            Type = newCallType,
                            From = from,
                            To = to,
                            RecordFile = recordFile,
                            LinphoneCallPtr = callref
                        };

                        Calls.AddOrUpdate(callref, existCall, (key, oldValue) => oldValue = existCall);

                        CallStateChangedEvent?.Invoke(existCall);
                    }
                }
            }
        }

        public void OnMessageReceived(IntPtr lc, IntPtr room, IntPtr message)
        {
            var peer_address = ChatModule.linphone_chat_room_get_peer_address(room);
            if (peer_address.IsNonZero())
            {
                var addressStringPtr = CallModule.linphone_address_as_string(peer_address);
                var chatMessagePtr = ChatModule.linphone_chat_message_get_text(message);

                string addressString, chatMessage;
                if (MarshalingExtensions.TryConvert(addressStringPtr, out addressString) && MarshalingExtensions.TryConvert(chatMessagePtr, out chatMessage))
                {
                    MessageReceivedEvent?.Invoke(addressString, chatMessage);
                }
            }
        }

        public void OnRegistrationChanged(IntPtr lc, IntPtr cfg, LinphoneRegistrationState cstate, string message)
        {
            if (LinphoneCore.IsNonZero())
            {
                logEventHandler?.Invoke("OnRegistrationChanged: " + cstate);

                RegistrationStateChangedEvent?.Invoke(cstate);
            }
        }

        public void SendMessage(string to, string message)
        {
            if (LinphoneCore.IsNonZero())
            {
                IntPtr chat_room = ChatModule.linphone_core_get_chat_room_from_uri(LinphoneCore, to);
                IntPtr chat_message = ChatModule.linphone_chat_room_create_message(chat_room, message);
                ChatModule.linphone_chat_room_send_chat_message(chat_room, chat_message);
            }
        }

        public void SetIncomingRingSound(string file)
        {
            if (LinphoneCore.IsNonZero())
            {
                MediaModule.linphone_core_set_ring(LinphoneCore, file);
            }
        }

        public void SetRingbackSound(string file)
        {
            if (LinphoneCore.IsNonZero())
            {
                MediaModule.linphone_core_set_ringback(LinphoneCore, file);
            }
        }

        public void TerminateCall(LinphoneCall linphoneCall)
        {
            if (linphoneCall.IsExist() && LinphoneCore.IsNonZero())
            {
                CallModule.linphone_core_terminate_call(LinphoneCore, linphoneCall.LinphoneCallPtr);
            }
        }

        public void TransferCall(LinphoneCall linphoneCall, string redirect_uri)
        {
            if (linphoneCall.IsExist() && LinphoneCore.IsNonZero())
            {
                CallModule.linphone_call_transfer(LinphoneCore, linphoneCall.LinphoneCallPtr, redirect_uri);
            }
        }

        /// <summary>
        /// Set speaker gain between 0..1f
        /// </summary>
        /// <param name="linphoneCall"></param>
        /// <param name="value"></param>
        public void SetSpeakerValue(LinphoneCall linphoneCall, double value)
        {
            if (linphoneCall.IsExist())
            {
                MediaModule.linphone_call_set_speaker_volume_gain(linphoneCall.LinphoneCallPtr, (float)value);
            }
        }

        public void EchoCancellation(LinphoneCall linphoneCall, bool value)
        {
            if (linphoneCall.IsExist())
            {
                MediaModule.linphone_call_enable_echo_cancellation(linphoneCall.LinphoneCallPtr, value);
            }
        }

        public double GetSpeakerSound(LinphoneCall linphoneCall)
        {
            var result = 0f;
            if (linphoneCall.IsExist())
            {
                result = MediaModule.linphone_call_get_speaker_volume_gain(linphoneCall.LinphoneCallPtr);
            }
            return result;
        }

        public void SendDTMFs(LinphoneCall linphoneCall, string dtmfs)
        {
            if (linphoneCall.IsExist() && LinphoneCore.IsNonZero() && !string.IsNullOrWhiteSpace(dtmfs))
            {
                CallModule.linphone_call_send_dtmfs(linphoneCall.LinphoneCallPtr, dtmfs);
            }
        }

        public void StartRecording(LinphoneCall linphoneCall)
        {
            if (linphoneCall.IsExist() && LinphoneCore.IsNonZero() && !string.IsNullOrWhiteSpace(linphoneCall.RecordFile))
            {
                GenericModules.linphone_call_start_recording(linphoneCall.LinphoneCallPtr);
            }
        }

        public void PauseRecording(LinphoneCall linphoneCall)
        {
            if (linphoneCall.IsExist() && LinphoneCore.IsNonZero() && !string.IsNullOrWhiteSpace(linphoneCall.RecordFile))
            {
                GenericModules.linphone_call_stop_recording(linphoneCall.LinphoneCallPtr);
            }
        }

        public void HoldCall(LinphoneCall linphoneCall)
        {
            if (linphoneCall.IsExist() && LinphoneCore.IsNonZero())
            {
                CallModule.linphone_core_pause_call(LinphoneCore, linphoneCall.LinphoneCallPtr);
            }
        }

        public void ResumeCall(LinphoneCall linphoneCall)
        {
            if (linphoneCall.IsExist() && LinphoneCore.IsNonZero())
            {
                CallModule.linphone_core_resume_call(LinphoneCore, linphoneCall.LinphoneCallPtr);
            }
        }

        public void RedirectCall(LinphoneCall linphoneCall, string redirect_uri)
        {
            if (linphoneCall.IsExist() && LinphoneCore.IsNonZero())
            {
                CallModule.linphone_call_redirect(LinphoneCore, linphoneCall.LinphoneCallPtr, redirect_uri);
            }
        }

        public void ReceiveCallAndRecord(LinphoneCall linphoneCall, string filename, bool startRecordInstantly = true)
        {
            if (linphoneCall.IsExist() && LinphoneCore.IsNonZero())
            {
                CallModule.linphone_call_ref(linphoneCall.LinphoneCallPtr);

                IntPtr callParams = CallParamsBuilder
                    .BuildAudioParams()
                    .BuildVideoParams()
                    .BuildMediaParams()
                    .Build();

                CallModule.linphone_call_params_set_record_file(callParams, filename);

                CallModule.linphone_core_accept_call_with_params(LinphoneCore, linphoneCall.LinphoneCallPtr, callParams);

                if (startRecordInstantly)
                {
                    GenericModules.linphone_call_start_recording(linphoneCall.LinphoneCallPtr);
                }
            }
        }

        public void ReceiveCall(LinphoneCall linphoneCall)
        {
            if (linphoneCall.IsExist() && LinphoneCore.IsNonZero())
            {
                CallModule.linphone_call_ref(linphoneCall.LinphoneCallPtr);

                IntPtr callParams = CallParamsBuilder
                    .BuildAudioParams()
                    .BuildVideoParams()
                    .BuildMediaParams()
                    .Build();

                CallModule.linphone_core_accept_call_with_params(LinphoneCore, linphoneCall.LinphoneCallPtr, callParams);
            }
        }

        #endregion

        #region Private Methods

        private LinphoneStructs.LinphoneCoreVTable CreateLinphoneCoreVTable()
        {
            return new LinphoneStructs.LinphoneCoreVTable()
            {
                global_state_changed = IntPtr.Zero,
                registration_state_changed = Marshal.GetFunctionPointerForDelegate(registration_state_changed),
                call_state_changed = Marshal.GetFunctionPointerForDelegate(call_state_changed),
                notify_presence_received = IntPtr.Zero,
                notify_presence_received_for_uri_or_tel = IntPtr.Zero,
                new_subscription_requested = IntPtr.Zero,
                auth_info_requested = IntPtr.Zero,
                authentication_requested = IntPtr.Zero,
                call_log_updated = IntPtr.Zero,
                message_received = Marshal.GetFunctionPointerForDelegate(message_received),
                message_received_unable_decrypt = IntPtr.Zero,
                is_composing_received = IntPtr.Zero,
                dtmf_received = IntPtr.Zero,
                refer_received = IntPtr.Zero,
                call_encryption_changed = IntPtr.Zero,
                transfer_state_changed = IntPtr.Zero,
                buddy_info_updated = IntPtr.Zero,
                call_stats_updated = IntPtr.Zero,
                info_received = IntPtr.Zero,
                subscription_state_changed = IntPtr.Zero,
                notify_received = IntPtr.Zero,
                publish_state_changed = IntPtr.Zero,
                configuring_status = IntPtr.Zero,
                display_status = IntPtr.Zero,
                display_message = IntPtr.Zero,
                display_warning = IntPtr.Zero,
                display_url = IntPtr.Zero,
                show = IntPtr.Zero,
                text_received = IntPtr.Zero,
                file_transfer_recv = IntPtr.Zero,
                file_transfer_send = IntPtr.Zero,
                file_transfer_progress_indication = IntPtr.Zero,
                network_reachable = IntPtr.Zero,
                log_collection_upload_state_changed = IntPtr.Zero,
                log_collection_upload_progress_indication = IntPtr.Zero,
                friend_list_created = IntPtr.Zero,
                friend_list_removed = IntPtr.Zero,
                user_data = IntPtr.Zero
            };
        }

        private void LinphoneMainLoop()
        {
            while (IsRunning)
            {
                CoreModule.linphone_core_iterate(LinphoneCore);
                Thread.Sleep(Constants.LC_SLEEP_TIMEOUT);
            }

            // Phone is disabled, free resources 

            NetworkModule.linphone_nat_policy_unref(NatPolicy);
            CoreModule.linphone_core_unref(LinphoneCore);

            VTablePtr.Free();
            TransportConfigPtr.Free();

            registration_state_changed = null;
            call_state_changed = null;

            LinphoneCore = IntPtr.Zero;
            ProxyCfg = IntPtr.Zero;
            AuthInfo = IntPtr.Zero;
            TransportConfigPtr = IntPtr.Zero;

            coreLoop = null;
            Identity = null;
            ServerHost = null;

            RegistrationStateChangedEvent?.Invoke(LinphoneRegistrationState.LinphoneRegistrationCleared);
        }

        private LinphoneStructs.LCSipTransports CreateTransportConfig()
        {
            return new LinphoneStructs.LCSipTransports()
            {
                udp_port = Constants.LC_SIP_TRANSPORT_RANDOM,
                tcp_port = Constants.LC_SIP_TRANSPORT_RANDOM,
                dtls_port = Constants.LC_SIP_TRANSPORT_RANDOM,
                tls_port = Constants.LC_SIP_TRANSPORT_RANDOM
            };
        }

        private IntPtr CreateNatPolicy(NatPolicy natPolicy)
        {
            var result = NetworkModule.linphone_core_create_nat_policy(LinphoneCore);
            result = NetworkModule.linphone_nat_policy_ref(result);

            NetworkModule.linphone_nat_policy_enable_stun(result, natPolicy.UseSTUN);
            NetworkModule.linphone_nat_policy_enable_turn(result, natPolicy.UseTURN);
            NetworkModule.linphone_nat_policy_enable_ice(result, natPolicy.UseICE);
            NetworkModule.linphone_nat_policy_enable_upnp(result, natPolicy.UseUPNP);

            if (!string.IsNullOrEmpty(natPolicy.Server))
            {
                NetworkModule.linphone_nat_policy_set_stun_server(result, natPolicy.Server);
                NetworkModule.linphone_nat_policy_resolve_stun_server(result);
            }

            return result;
        }

        private IntPtr CreateProxyCfg()
        {
            var result = ProxieModule.linphone_core_create_proxy_config(LinphoneCore);

            ProxieModule.linphone_proxy_config_set_identity(result, Identity);
            ProxieModule.linphone_proxy_config_set_server_addr(result, ServerHost);
            ProxieModule.linphone_proxy_config_enable_register(result, true);

            ProxieModule.linphone_proxy_config_set_nat_policy(result, NatPolicy);

            ProxieModule.linphone_core_add_proxy_config(LinphoneCore, result);
            ProxieModule.linphone_core_set_default_proxy_config(LinphoneCore, result);

            return result;
        }

        private static async Task ExecuteWithDelay(Action action, int timeoutInMilliseconds)
        {
            await Task.Delay(timeoutInMilliseconds);
            action();
        }



        #endregion
    }
}
