using lindotnet.Classes.Component.Implementation;
using lindotnet.Classes.Helpers;
using lindotnet.Classes.Wrapper.Implementation.Loader;
using lindotnet.Classes.Wrapper.Implementation.Modules;
using lindotnet.Classes.Wrapper.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace lindotnet.Classes.Wrapper.Implementation
{
	public class LinphoneWrapper : ILinphoneWrapper
	{
		#region Fields

		private LinphoneDelegates.LinphoneCoreRegistrationStateChanged _registrationStateChanged;

		private LinphoneDelegates.LinphoneCoreCallStateChanged _callStateChanged;

		private LinphoneDelegates.LinphoneCoreCbsMessageReceived _messageReceived;

		private Thread _coreLoop;

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

		#region Events

		public event RegistrationStateChangedDelegate RegistrationStateChangedEvent;

		public event CallStateChangedDelegate CallStateChangedEvent;

		public event MessageReceivedDelegate MessageReceivedEvent;

		#endregion

		#region Delegates

		public delegate void RegistrationStateChangedDelegate(LinphoneRegistrationState state, string message = null);

		public delegate void CallStateChangedDelegate(Call call);

		public delegate void ErrorDelegate(Call call, string message);

		public delegate void MessageReceivedDelegate(string from, string message);

		#endregion

		static LinphoneWrapper()
		{
			SetUpModuleContainer();
		}

		private static void SetUpModuleContainer()
		{
			ModuleContainer.SetModule<CoreModule>(new CoreModule());
			ModuleContainer.SetModule<GenericModules>(new GenericModules());
			ModuleContainer.SetModule<NetworkModule>(new NetworkModule());
			ModuleContainer.SetModule<ProxieModule>(new ProxieModule());
			ModuleContainer.SetModule<MediaModule>(new MediaModule());
			ModuleContainer.SetModule<CallModule>(new CallModule());
			ModuleContainer.SetModule<ChatModule>(new ChatModule());
		}

		public LinphoneWrapper()
		{
			Calls = new ConcurrentDictionary<IntPtr, LinphoneCall>();
			CoreModule.linphone_core_set_log_level(OrtpLogLevel.END);
		}

		#region Interface Imlementation

		public void CreatePhone(LinphoneConnectionParams connectionParams)
		{
			IsRunning = true;

			_registrationStateChanged = new LinphoneDelegates.LinphoneCoreRegistrationStateChanged(OnRegistrationChanged);
			_callStateChanged = new LinphoneDelegates.LinphoneCoreCallStateChanged(OnCallStateChanged);
			_messageReceived = new LinphoneDelegates.LinphoneCoreCbsMessageReceived(OnMessageReceived);

			VTable = CreateDefaultLinphoneCoreVTable();
			VTablePtr = VTable.ToIntPtr();

			LinphoneCore = CreateLinphoneCore();

			_coreLoop = new Thread(LinphoneMainLoop)
			{
				IsBackground = false
			};
			_coreLoop.Start();

			TransportConfig = CreateTransportConfig();
			TransportConfigPtr = TransportConfig.ToIntPtr();
			NetworkModule.linphone_core_set_sip_transports(LinphoneCore, TransportConfigPtr);

			GenericModules.linphone_core_set_user_agent(LinphoneCore, connectionParams.Agent, connectionParams.Version);

			Identity = string.IsNullOrEmpty(connectionParams.AccountAlias)
				? $"sip:{connectionParams.Username}@{connectionParams.Server}"
				: $"\"{connectionParams.AccountAlias}\" sip:{connectionParams.Username}@{connectionParams.Server}";

			ServerHost = string.IsNullOrWhiteSpace(connectionParams.ProxyHost)
				? $"sip:{connectionParams.Server}:{connectionParams.Port}"
				: $"sip:{connectionParams.ProxyHost}:{connectionParams.Port}";

			AuthInfo = GenericModules.linphone_auth_info_new(connectionParams.Username, null, connectionParams.Password, null, null, null);
			GenericModules.linphone_core_add_auth_info(LinphoneCore, AuthInfo);

			NatPolicy = CreateNatPolicy(connectionParams.NatPolicy);

			ProxyCfg = CreateProxyCfg();

			CallParamsBuilder = new CallParamsBuilder(LinphoneCore);
		}

		/// <summary>
		/// Creates a new LinphonCore instance
		/// </summary>
		/// <returns></returns>
		private IntPtr CreateLinphoneCore()
		{
			var result = IntPtr.Zero;

			// Deprecated Now, use factory methods
			//result = CoreModule.linphone_core_new(VTablePtr, null, null, IntPtr.Zero);

			// Factory methods now not implemented, some issues 
			// in subscription on changing state
			//IntPtr factory = CoreModule.linphone_factory_get();
			//IntPtr cbs = CoreModule.linphone_factory_create_core_cbs(factory);
			//result = CoreModule.linphone_factory_create_core(factory, cbs);

			return result;
		}

		public void DestroyPhone()
		{
			if (LinphoneCore != null)
			{
				RegistrationStateChangedEvent?.Invoke(LinphoneRegistrationState.LinphoneRegistrationProgress);

				CallModule.linphone_core_terminate_all_calls(LinphoneCore);

				var proxySetDownTask = ComponentExtensions.ExecuteWithDelay(() =>
				{
					if (ProxieModule.linphone_proxy_config_is_registered(ProxyCfg))
					{
						ProxieModule.linphone_proxy_config_edit(ProxyCfg);
						ProxieModule.linphone_proxy_config_enable_register(ProxyCfg, false);
						ProxieModule.linphone_proxy_config_done(ProxyCfg);
					}
				}, Constants.LC_CORE_PROXY_DISABLE_TIMEOUT);

				proxySetDownTask.Wait(Constants.LC_CORE_PROXY_DISABLE_TIMEOUT);

				IsRunning = ProxieModule.linphone_proxy_config_is_registered(ProxyCfg);
			}
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
				var newCallType = CallType.None;
				string from, to, recordFile;

				from = to = recordFile = null;
				IntPtr callParams = CallModule.linphone_call_get_params(call);

				bool recordEnable = MarshalingExtensions.TryConvertString(CallModule.linphone_call_params_get_record_file(callParams), out recordFile);

				var newCallState = GetNewCallState(call, callState, ref newCallType, ref from, ref to, recordEnable);

				UpdateCallReferences(call, newCallState, newCallType, callState, from, to, recordFile);
			}
		}

		private CallState GetNewCallState(IntPtr call, LinphoneCallState callState,
			ref CallType newCallType, ref string from, ref string to, bool recordEnable)
		{
			var newCallState = CallState.None;
			switch (callState)
			{
				case LinphoneCallState.LinphoneCallIncomingReceived:
				case LinphoneCallState.LinphoneCallIncomingEarlyMedia:
					newCallState = CallState.Loading;
					newCallType = CallType.Incoming;
					MarshalingExtensions.TryConvertString(CallModule.linphone_call_get_remote_address_as_string(call), out from);
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
					MarshalingExtensions.TryConvertString(CallModule.linphone_call_get_remote_address_as_string(call), out to);
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
			}

			return newCallState;
		}

		private void UpdateCallReferences(IntPtr call, CallState newCallState, CallType newCallType,
			LinphoneCallState callState, string from, string to, string recordFile)
		{
			IntPtr callref = CallModule.linphone_call_ref(call);
			LinphoneCall existCall = null;

			if (callref.IsNonZero())
			{
				if (Calls.TryGetValue(callref, out existCall))
				{
					if (existCall.State != newCallState)
					{
						existCall.State = newCallState;
						CallStateChangedEvent?.Invoke(existCall);
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

					Calls.TryAdd(callref, existCall);
					CallStateChangedEvent?.Invoke(existCall);
				}
			}

			if (callState == LinphoneCallState.LinphoneCallReleased)
			{
				CallModule.linphone_call_unref(existCall.LinphoneCallPtr);
				Calls.TryRemove(callref, out existCall);
				return;
			}
		}

		public void OnMessageReceived(IntPtr core, IntPtr room, IntPtr message)
		{
			var peer_address = ChatModule.linphone_chat_room_get_peer_address(room);
			if (peer_address.IsNonZero())
			{
				var addressStringPtr = CallModule.linphone_address_as_string(peer_address);
				var chatMessagePtr = ChatModule.linphone_chat_message_get_text(message);

				if (MarshalingExtensions.TryConvertString(addressStringPtr, out string addressString) && MarshalingExtensions.TryConvertString(chatMessagePtr, out string chatMessage))
				{
					MessageReceivedEvent?.Invoke(addressString, chatMessage);
				}
			}
		}

		public void OnRegistrationChanged(IntPtr core, IntPtr cfg, LinphoneRegistrationState state, string message)
		{
			if (LinphoneCore.IsNonZero())
			{
				RegistrationStateChangedEvent?.Invoke(state, message);
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

		private LinphoneStructs.LinphoneCoreVTable CreateDefaultLinphoneCoreVTable()
		{
			return new LinphoneStructs.LinphoneCoreVTable()
			{
				global_state_changed = IntPtr.Zero,
				registration_state_changed = Marshal.GetFunctionPointerForDelegate(_registrationStateChanged),
				call_state_changed = Marshal.GetFunctionPointerForDelegate(_callStateChanged),
				notify_presence_received = IntPtr.Zero,
				notify_presence_received_for_uri_or_tel = IntPtr.Zero,
				new_subscription_requested = IntPtr.Zero,
				auth_info_requested = IntPtr.Zero,
				authentication_requested = IntPtr.Zero,
				call_log_updated = IntPtr.Zero,
				message_received = Marshal.GetFunctionPointerForDelegate(_messageReceived),
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

			_registrationStateChanged = null;
			_callStateChanged = null;

			LinphoneCore = IntPtr.Zero;
			ProxyCfg = IntPtr.Zero;
			AuthInfo = IntPtr.Zero;
			TransportConfigPtr = IntPtr.Zero;

			_coreLoop = null;
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

		#endregion

		~LinphoneWrapper()
		{
			if (IsRunning)
			{
				DestroyPhone();
			}
		}
	}
}
