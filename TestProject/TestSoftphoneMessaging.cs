using System;
using System.Threading.Tasks;
using lindotnet.Classes;
using lindotnet.Classes.Component.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
	[TestClass]
	public class TestSoftphoneMessaging
	{
		private static readonly TimeSpan ConnectionDelay = TimeSpan.FromSeconds(2);
		private static readonly string ExampleURI = "100";

		[TestMethod]
		public void TestSofpthoneSendMessage()
		{
			Softphone softphoneInstance = null;
			string mock = "MockMockMock 012456789";
			try
			{
				var testAccount = new Account(
					login: "test",
					password: "testpass",
					host: "192.168.156.2",
					accountName: "TestUser");

				softphoneInstance = new Softphone(testAccount);

				softphoneInstance.Connect();

				Task.Delay(ConnectionDelay).Wait();

				if(softphoneInstance.ConnectionState == ConnectState.Connected)
				{
					softphoneInstance.SendMessage(ExampleURI, mock);
				}
				else
				{
					throw new LinphoneException("Phone not connected!");
				}
			}
			finally
			{
				softphoneInstance.Disconnect();
			}			
		}
	}
}
