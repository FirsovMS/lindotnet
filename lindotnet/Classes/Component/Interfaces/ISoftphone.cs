using lindotnet.Classes.Component.Implementation;

namespace lindotnet.Classes.Component.Interfaces
{
	public interface ISoftphone
	{
		void MakeCall(string uri);

		void ReceiveCall(Call call);

		void TerminateCall(Call call);

		void HoldCall(Call call);

		void ResumeCall(Call call);

		void RedirectCall(Call call, string redirectURI);

		void TransferCall(Call call, string redirectURI);

		void MakeCallManualRecord(string sipUriOrPhone, string filename);

		void MakeCallAndRecord(string sipUriOrPhone, string filename);

		void ReceiveCallManualRecord(Call call, string filename);

		void ReceiveCallAndRecord(Call call, string filename);

		void SendMessage(string to, string message);

		void StartRecording(Call call);

		void PauseRecording(Call call);

		void SendDTMFs(Call call, string dtmfs);

		void SetIncomingRingSound(string filename);

		void SetRingbackSound(string filename);
	}
}