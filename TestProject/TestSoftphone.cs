using lindotnet.Classes.Component.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace TestProject
{
	[TestClass]
	public class TestSoftphone
	{
		private static readonly TimeSpan ConnectionDelay = TimeSpan.FromSeconds(2);
		private static readonly string ExampleURI = "music@iptel.org";

		[TestMethod]
		public void CreateSoftphoneInstance()
		{
			// using office sip for testing
			var testAccount = new Account(
				login: "test",
				password: "testpass",
				host: "192.168.156.2",
				accountName: "TestUser");

			var softphoneInstance = new Softphone(testAccount);
		}

		[TestMethod]
		public void TestSoftphoneServerConnection()
		{
			Softphone softphoneInstance = null;
			try
			{
				// using office sip for testing
				var testAccount = new Account(
					login: "test",
					password: "testpass",
					host: "192.168.156.2",
					accountName: "TestUser");

				softphoneInstance = new Softphone(testAccount);

				softphoneInstance.Connect();

				Task.Delay(ConnectionDelay).Wait();

				Assert.AreEqual(lindotnet.Classes.ConnectState.Connected, softphoneInstance.ConnectState);

			}
			catch (Exception e)
			{
				Assert.Fail(e.Message);
			}
			finally
			{
				softphoneInstance?.Disconnect();
			}
		}

		[TestMethod]
		public void TestCallToAnotherSoftphone()
		{
			Softphone softphoneInstance = null;
			try
			{
				// using office sip for testing
				var testAccount = new Account(
					login: "test",
					password: "testpass",
					host: "192.168.156.2",
					accountName: "TestUser");

				softphoneInstance = new Softphone(testAccount);

				softphoneInstance.Connect();

				Task.Delay(ConnectionDelay).Wait();

				softphoneInstance.MakeCall(ExampleURI);

				Assert.AreEqual(lindotnet.Classes.LineState.Busy, softphoneInstance.LineState);
				if(softphoneInstance.ActiveCall != null)
				{

				}
			}
			catch (Exception e)
			{
				Assert.Fail(e.Message);
			}
			finally
			{
				softphoneInstance?.Disconnect();
			}
		}
	}
}
