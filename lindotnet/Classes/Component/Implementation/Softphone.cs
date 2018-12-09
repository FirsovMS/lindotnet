using lindotnet.Classes.Component.Interfaces;
using lindotnet.Classes.Wrapper.Implementation;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace lindotnet.Classes.Component.Implementation
{
	public class Softphone : SoftphoneBase, ISoftphone
	{
		#region Props

		public Media MediaController { get; private set; }

		public LinphoneCall ActiveCall { get; private set; }

		#endregion

		public Softphone(Account account) : base(account)
		{
			MediaController = new Media(this);

			LinphoneWrapper.RegistrationStateChangedEvent += LinphoneWrapper_RegistrationStateChangedEvent;

			LinphoneWrapper.CallStateChangedEvent += LinphoneWrapper_CallStateChangedEvent;

			LinphoneWrapper.MessageReceivedEvent += LinphoneWrapper_MessageReceivedEvent;
		}

		#region Events

		/// <summary>
		/// Successful registered
		/// </summary>
		public delegate void OnPhoneConnected();

		/// <summary>
		/// Successful unregistered
		/// </summary>
		public delegate void OnPhoneDisconnected();

		/// <summary>
		/// Phone is ringing
		/// </summary>
		/// <param name="call"></param>
		public delegate void OnIncomingCall(Call call);

		/// <summary>
		/// Link is established
		/// </summary>
		/// <param name="call"></param>
		public delegate void OnCallActive(Call call);

		/// <summary>
		/// Call completed
		/// </summary>
		/// <param name="call"></param>
		public delegate void OnCallCompleted(Call call);

		/// <summary>
		/// Message received
		/// </summary>
		/// <param name="call"></param>
		public delegate void OnMessageReceived(string from, string message);

		/// <summary>
		/// Error notification
		/// </summary>
		/// <param name="call"></param>
		/// <param name="error"></param>
		public delegate void OnError(Call call, Error error);

		/// <summary>
		/// Call Holded
		/// </summary>
		/// <param name="call"></param>
		public delegate void OnHold(Call call);

		/// <summary>
		/// Raw log notification
		/// </summary>
		/// <param name="message"></param>
		public delegate void OnLog(string message);

		public event OnPhoneConnected PhoneConnectedEvent;

		public event OnPhoneDisconnected PhoneDisconnectedEvent;

		public event OnIncomingCall IncomingCallEvent;

		public event OnCallActive CallActiveEvent;

		public event OnCallCompleted CallCompletedEvent;

		public event OnMessageReceived MessageReceivedEvent;

		public event OnError ErrorEvent;

		public event OnHold CallHolded;

		#endregion

		#region Implement interface

		public void HoldCall(Call call)
		{
			CheckError(call);
			var linphonceCall = call as LinphoneCall;
			if (linphonceCall != null)
			{
				LinphoneWrapper.HoldCall(linphonceCall);
			}
		}

		public void MakeCall(string uri)
		{
			CheckError();
			CheckError(uri);
			if (LineState == LineState.Free)
			{
				LinphoneWrapper.MakeCall(uri);
			}
		}

		public void MakeCallAndRecord(string uri, string filename, bool recordStartInstantly = false)
		{
			CheckError();
			CheckError(filename, uri);
			if (LineState == LineState.Free)
			{
				LinphoneWrapper.MakeCallAndRecord(uri, filename, recordStartInstantly);
			}
		}

		public void PauseRecording(Call call)
		{
			CheckError(call);
			var linphoneCall = call as LinphoneCall;
			if (linphoneCall != null)
			{
				LinphoneWrapper.PauseRecording(linphoneCall);
			}
		}

		public void ReceiveCall(Call call)
		{
			CheckError(call);
			var linphoneCall = call as LinphoneCall;
			if (linphoneCall != null)
			{
				LinphoneWrapper.ReceiveCall(linphoneCall);
			}
		}

		public void ReceiveCallAndRecord(Call call, string filename, bool recordStartInstantly = false)
		{
			CheckError(call, filename);
			var linphoneCall = call as LinphoneCall;
			if (linphoneCall != null)
			{
				LinphoneWrapper.ReceiveCallAndRecord(linphoneCall, filename, recordStartInstantly);
			}
		}

		public void RedirectCall(Call call, string redirectURI)
		{
			CheckError(call, redirectURI);
			var linphoneCall = call as LinphoneCall;
			if (linphoneCall != null)
			{
				LinphoneWrapper.RedirectCall(linphoneCall, redirectURI);
			}
		}

		public void ResumeCall(Call call)
		{
			CheckError(call);
			var linphoneCall = call as LinphoneCall;
			if (linphoneCall != null)
			{
				LinphoneWrapper.ResumeCall(linphoneCall);
			}
		}

		public void SendDTMFs(Call call, string dtmfs)
		{
			CheckError(call, dtmfs);
			var linphoneCall = call as LinphoneCall;
			if (linphoneCall != null)
			{
				LinphoneWrapper.SendDTMFs(linphoneCall, dtmfs);
			}
		}

		public void SendMessage(string to, string message)
		{
			CheckError();
			CheckError(to, message);
			LinphoneWrapper.SendMessage(to, message);
		}

		public void SetIncomingRingSound(string filename)
		{
			CheckError();
			CheckError(filename);
			LinphoneWrapper.SetIncomingRingSound(filename);
		}

		public void SetRingbackSound(string filename)
		{
			CheckError();
			CheckError(filename);
			LinphoneWrapper.SetRingbackSound(filename);
		}

		public void StartRecording(Call call)
		{
			CheckError(call);
			var linphoneCall = call as LinphoneCall;
			if (linphoneCall != null)
			{
				LinphoneWrapper.StartRecording(linphoneCall);
			}
		}

		public void TerminateCall(Call call)
		{
			CheckError(call);
			var linphoneCall = call as LinphoneCall;
			if (linphoneCall != null)
			{
				LinphoneWrapper.TerminateCall(linphoneCall);
			}
		}

		public void TransferCall(Call call, string redirectURI)
		{
			CheckError(call, redirectURI);
			var linphoneCall = call as LinphoneCall;
			if (linphoneCall != null)
			{
				LinphoneWrapper.TransferCall(linphoneCall, redirectURI);
			}
		}

		#endregion

		#region Private Methods

		private void LinphoneWrapper_MessageReceivedEvent(string from, string message)
		{
			MessageReceivedEvent?.Invoke(from, message);
		}

		private void LinphoneWrapper_CallStateChangedEvent(Call call)
		{
			var callState = call.State;

			switch (callState)
			{
				case CallState.Active:
					LineState = LineState.Busy;
					CallActiveEvent?.Invoke(call);
					break;
				case CallState.Hold:
					break;
				case CallState.Error:
					LineState = LineState.Free;
					ErrorEvent?.Invoke(null, Error.CallError);
					break;
				case CallState.Loading:
					LineState = LineState.Busy;
					if (call.Type == CallType.Incoming)
					{
						IncomingCallEvent?.Invoke(call);
					}
					break;
				case CallState.Completed:
				default:
					LineState = LineState.Free;
					CallCompletedEvent?.Invoke(call);
					break;
			}
		}

		private void LinphoneWrapper_RegistrationStateChangedEvent(LinphoneRegistrationState state)
		{
			switch (state)
			{
				case LinphoneRegistrationState.LinphoneRegistrationProgress:
					ConnectState = ConnectState.Progress;
					break;

				case LinphoneRegistrationState.LinphoneRegistrationOk:
					ConnectState = ConnectState.Connected;
					PhoneConnectedEvent?.Invoke();
					break;

				case LinphoneRegistrationState.LinphoneRegistrationCleared:
					ConnectState = ConnectState.Disconnected;
					PhoneDisconnectedEvent?.Invoke();
					break;

				case LinphoneRegistrationState.LinphoneRegistrationFailed:
					LinphoneWrapper.DestroyPhone();
					ErrorEvent?.Invoke(null, Error.RegisterFailed);
					break;

				case LinphoneRegistrationState.LinphoneRegistrationNone:
				default:
					break;
			}
		}

		private void CheckError()
		{
			if (ConnectState != ConnectState.Connected)
			{
				throw new LinphoneException("Softphone didn't connected!");
			}
			if (LinphoneWrapper == null || LinphoneWrapper?.IsRunning == false)
			{
				throw new LinphoneException("Wrapper didn't ready!");
			}
		}

		private void CheckError(Call call)
		{
			CheckError();
			if (call == null)
			{
				throw new LinphoneException("Call can't be null!");
			}
		}

		private void CheckError(Call call, params string[] additionalStringParams)
		{
			CheckError(call);
			CheckError(additionalStringParams);
		}

		private void CheckError(params string[] additionalStringParams)
		{
			if (additionalStringParams.Any(str => string.IsNullOrWhiteSpace(str)))
			{
				throw new ArgumentException("Additional string must be not null, or empty, or whitespace!");
			}
		}

		#endregion

		~Softphone()
		{
			base.Disconnect();
		}
	}
}
