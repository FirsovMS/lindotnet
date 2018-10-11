using lindotnet.Classes.Component.Implementation;

namespace lindotnet.Classes.Wrapper.Implementation
{
	public struct LinphoneConnectionParams
	{
		public string username;

		public string accountAlias;

		public string password;

		public string server;

		public int port;

		public string agent;

		public string version;

		public NatPolicy natPolicy;
	}
}