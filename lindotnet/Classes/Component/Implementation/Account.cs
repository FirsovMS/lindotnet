namespace lindotnet.Classes.Component.Implementation
{
	public class Account
	{
		#region Props

		public string Username { get; }

		public string AccountName { get; }

		public string Password { get; }

		public string Server { get; }

		public int Port { get; }

		public string ProxyHost { get; }

		public int Id { get; set; }

		public string Identiny
		{
			get
			{
				return $"sip:{Username}@{Server}";
			}
		}

		#endregion

		/// <summary>
		/// Create User Account
		/// </summary>
		/// <param name="login"></param>
		/// <param name="password"></param>
		/// <param name="server"></param>
		/// <param name="proxyHost"></param>
		/// <param name="port"></param>
		/// <param name="accountName"></param>
		public Account(string login, string password, string server, string proxyHost = null, int port = 5060, string accountName = null)
		{
			Username = login;
			AccountName = accountName;
			Password = password;
			Server = server;
			Port = port;
			ProxyHost = proxyHost;
		}
	}
}
