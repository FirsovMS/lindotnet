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

		void MakeCallAndRecord(string uri, string filename, bool recordStartInstantly = true);

		void ReceiveCallAndRecord(Call call, string filename, bool recordStartInstantly = true);

		void SendMessage(string to, string message);

		void StartRecording(Call call);

		void PauseRecording(Call call);

		void SendDTMFs(Call call, string dtmfs);

		void SetIncomingRingSound(string filename);

		void SetRingbackSound(string filename);
	}
}