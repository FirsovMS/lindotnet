using lindotnet.Classes.Component.Interfaces;
using lindotnet.Classes.Wrapper.Implementation;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace lindotnet.Classes.Component.Implementation
{
	public abstract class SoftphoneBase : ISoftphoneBase
	{
		private readonly ConcurrentDictionary<string, string> errors;

		#region Props

		public bool HasErrors { get => errors.Any(); }

		public ConnectState ConnectionState { get; protected set; }

		public LineState LineState { get; set; }

		public Account Account { get; set; }

		public string UserAgent { get; set; } = Constants.DefaultUserAgent;

		public Version Version { get; } = Constants.ClientVersion;

		public LinphoneWrapper LinphoneWrapper { get; private set; }

		#endregion

		public SoftphoneBase(Account account)
		{
			errors = new ConcurrentDictionary<string, string>();

			Account = account ?? throw new LinphoneException("Softphone requires as Account to make calls!");
			LinphoneWrapper = new LinphoneWrapper();
		}

		#region Interface Implementation

		public virtual void Connect()
		{
			Connect(NatPolicy.GetDefaultNatPolicy());
		}

		public virtual void Connect(NatPolicy natPolicy)
		{
			if (ConnectionState == ConnectState.Disconnected)
			{
				ConnectionState = ConnectState.Progress;

				var connParams = CreateConnectionParams(Account, natPolicy);
				LinphoneWrapper.CreatePhone(connParams);
			}
		}

		public virtual void Disconnect()
		{
			if (ConnectionState == ConnectState.Connected)
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
				Agent = UserAgent,
				Version = Version.ToString(),
				NatPolicy = natPolicy
			};
		}

		~SoftphoneBase()
		{
			Disconnect();
		}
	}
}
