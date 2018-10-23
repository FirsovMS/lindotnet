using lindotnet.Classes.Component.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace TestProject
{
	[TestClass]
	public class BaseTests
	{
		private static readonly TimeSpan ConnectionDelay = TimeSpan.FromSeconds(2);

		[TestMethod]
		public void CreateInstances()
		{
			// using office sip for testing
			var testAccount = new Account(
				login: "test",
				password: "testpass",
				host: "officesip.local",
				accountName: "TestUser");

			var softphoneInstance = new Softphone(testAccount);
		}

		[TestMethod]
		public void TestPhoneConnectionOnServer()
		{
			// using local sip server for testing
			var testAccount = new Account(
				login: "test",
				password: "testpass",
				host: "192.168.156.2",
				accountName: "TestUser");

			var softphoneInstance = new Softphone(testAccount);

			try
			{
				softphoneInstance.Connect();

				var delayTask = Task.Delay(ConnectionDelay);
				delayTask.Wait();

				Assert.AreEqual(lindotnet.Classes.ConnectState.Connected, softphoneInstance.ConnectState);
			}
			catch (Exception e)
			{
				Assert.Fail($"TestPhoneConnectionOnServer Failed: {e.Message}");
			}
			finally
			{
				softphoneInstance.Disconnect();
			}
		}

		[TestMethod]
		public void TestCallToAnotherSoftphone()
		{
			// using local sip server for testing
			var testAccount = new Account(
				login: "test",
				password: "testpass",
				host: "192.168.156.2",
				accountName: "TestUser");

			var softphoneInstance = new Softphone(testAccount);

			try
			{
				softphoneInstance.Connect();

				var delayTask = Task.Delay(ConnectionDelay);
				delayTask.Wait();

				softphoneInstance.MakeCall("100");

				Assert.AreEqual(lindotnet.Classes.ConnectState.Connected, softphoneInstance.ConnectState);
			}
			catch (Exception e)
			{
				Assert.Fail($"TestPhoneConnectionOnServer Failed: {e.Message}");
			}
			finally
			{
				softphoneInstance.Disconnect();
			}
		}
	}
}
