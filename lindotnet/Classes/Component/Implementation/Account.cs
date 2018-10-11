namespace lindotnet.Classes.Component.Implementation
{
	public class Account
	{
		#region Props

		public string Username { get; set; }

		public string AccountName { get; set; }

		public string Password { get; set; }

		public string Host { get; set; }

		public int Port { get; set; }

		public int Id { get; set; }

		public string Identiny => $"sip:{Username}@{Host}";

		#endregion

		/// <summary>
		/// Create User Account
		/// </summary>
		/// <param name="login"></param>
		/// <param name="password"></param>
		/// <param name="host"></param>
		/// <param name="port"></param>
		/// <param name="accountName"></param>
		public Account(string login, string password, string host, int port = 5060, string accountName = "")
		{
			Username = login;
			AccountName = accountName;
			Password = password;
			Host = host;
			Port = port;
		}
	}
}
