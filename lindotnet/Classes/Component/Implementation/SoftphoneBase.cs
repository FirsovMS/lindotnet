using lindotnet.Classes.Component.Interfaces;
using lindotnet.Classes.Wrapper.Implementation;

namespace lindotnet.Classes.Component.Implementation
{
	public abstract class SoftphoneBase : ISoftphoneBase
	{
		#region Props

		public ConnectState ConnectState { get; set; }

		public LineState LineState { get; set; }

		public Account Account { get; set; }

		public string Useragent { get; set; } = Constants.DefaultUserAgent;

		public string Version { get; set; } = Constants.ClientVersion;

		public LinphoneWrapper LinphoneWrapper { get; private set; }

		#endregion

		public SoftphoneBase(Account account)
		{
			Account = account ?? throw new LinphoneException("Softphone requires as Account to make calls!");
			LinphoneWrapper = new LinphoneWrapper();
		}

		#region Interface Implementation

		public void Connect()
		{
			Connect(NatPolicy.GetDefaultNatPolicy());
		}

		public void Connect(NatPolicy natPolicy)
		{
			if (ConnectState == ConnectState.Disconnected)
			{
				ConnectState = ConnectState.Progress;

				var connParams = CreateConnectionParams(Account, natPolicy);
				LinphoneWrapper.CreatePhone(connParams);
			}
		}

		public void Disconnect()
		{
			if (ConnectState == ConnectState.Connected)
			{
				LinphoneWrapper.DestroyPhone();
			}
		}

		#endregion

		private LinphoneConnectionParams CreateConnectionParams(Account account, NatPolicy natPolicy)
		{
			return new LinphoneConnectionParams()
			{
				Username = Account.Username,
				Password = Account.Password,
				AccountAlias = Account.AccountName,
				Host = Account.Host,
				Port = Account.Port,
				Agent = Useragent,
				Version = Version,
				NatPolicy = natPolicy
			};
		}

		~SoftphoneBase()
		{
			Disconnect();
		}
	}
}
