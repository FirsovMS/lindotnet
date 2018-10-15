using System;
using System.Collections.Generic;
using lindotnet.Classes.Wrapper.Implementation;

namespace lindotnet.Classes.Wrapper.Interfaces
{
	internal interface ILinphoneWrapper
	{
		void CreatePhone(LinphoneConnectionParams connectionParams);

		void DestroyPhone();

		void SetSpeakerValue(LinphoneCall linphoneCall, double value);

		void EchoCancellation(LinphoneCall linphoneCall, bool val);

		double GetSpeakerSound(LinphoneCall linphoneCall);

		void SendDTMFs(LinphoneCall linphoneCall, string dtmfs);

		void SetRingbackSound(string file);

		void SetIncomingRingSound(string file);

		void TerminateCall(LinphoneCall linphoneCall);

		void MakeCall(string uri);

		void MakeCallAndRecord(string uri, string filename, bool startRecordInstantly = true);

		void StartRecording(LinphoneCall linphoneCall);

		void PauseRecording(LinphoneCall linphoneCall);

		void HoldCall(LinphoneCall linphoneCall);

		void ResumeCall(LinphoneCall linphoneCall);

		void RedirectCall(LinphoneCall linphoneCall, string redirect_uri);

		void TransferCall(LinphoneCall linphoneCall, string redirect_uri);

		void ReceiveCallAndRecord(LinphoneCall linphoneCall, string filename, bool startRecordInstantly = true);

		void ReceiveCall(LinphoneCall linphoneCall);

		void SendMessage(string to, string message);

		void OnRegistrationChanged(IntPtr lc, IntPtr cfg, LinphoneRegistrationState cstate, string message);

		void OnCallStateChanged(IntPtr lc, IntPtr call, LinphoneCallState cstate, string message);

		void LinphoneLogEvent(string domain, OrtpLogLevel lev, string fmt, IntPtr args);

		void OnMessageReceived(IntPtr lc, IntPtr room, IntPtr message);
	}
}
