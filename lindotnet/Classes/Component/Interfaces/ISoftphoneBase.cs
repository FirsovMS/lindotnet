using lindotnet.Classes.Component.Implementation;

namespace lindotnet.Classes.Component.Interfaces
{
	internal interface ISoftphoneBase
	{
		void Connect();

		void Connect(NatPolicy natPolicy);

		void Disconnect();
	}
}