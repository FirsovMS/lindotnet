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
		private Softphone _softphoneInstance;

		[ClassInitialize]
		private void BeforeTestsStart()
		{
			var testAccount = new Account(
					login: "test",
					password: "testpass",
					host: "192.168.156.2",
					accountName: "TestUser");

			_softphoneInstance = new Softphone(testAccount);

			_softphoneInstance.Connect();

			Task.Delay(ConnectionDelay).Wait();
		}

		[TestMethod]
		public void TestSofpthoneSendMessage()
		{
			_softphoneInstance.SendMessage(ExampleURI, "SMOCKING MOCK!");
		}

		[ClassCleanup]
		private void AfterTests()
		{
			_softphoneInstance?.Disconnect();
		}
	}
}
