using lindotnet.Classes.Component.Implementation;

namespace lindotnet.Classes.Component.Interfaces
{
	public interface ISoftphoneBase
	{
		/// <summary>
		/// Try Connect to SIP Server
		/// </summary>
		void Connect();

		void Connect(NatPolicy natPolicy);

		void Disconnect();
	}
}