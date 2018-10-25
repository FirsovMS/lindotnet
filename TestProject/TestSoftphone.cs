using lindotnet.Classes.Component.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TestProject
{
	[TestClass]
	public class TestSoftphone
	{
		private static readonly string workingDir = Environment.CurrentDirectory + @"\records";
		private static readonly TimeSpan ConnectionDelay = TimeSpan.FromSeconds(2);
		private static readonly TimeSpan CallTime = TimeSpan.FromSeconds(5);
		private static readonly string ExampleURI = "100";

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

			Assert.IsNotNull(softphoneInstance);
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
			finally
			{
				softphoneInstance.Disconnect();
			}
		}

		//[TestMethod]
		//public void TestCallAndRecordToAnotherSoftphone()
		//{
		//	Softphone softphoneInstance = null;
		//	bool isCallEnded = false;
		//	try
		//	{
		//		if (!Directory.Exists(workingDir))
		//		{
		//			Directory.CreateDirectory(workingDir);
		//		}
		//		string outFile = workingDir + @"\mock_record.wav";

		//		// using office sip for testing
		//		var testAccount = new Account(
		//			login: "test",
		//			password: "testpass",
		//			host: "192.168.156.2",
		//			accountName: "TestUser");

		//		Call currentCall = null;
		//		softphoneInstance = new Softphone(testAccount);

		//		softphoneInstance.CallActiveEvent += (call) =>
		//		{
		//			currentCall = call;
		//		};

		//		softphoneInstance.CallCompletedEvent += (call) =>
		//		{
		//			isCallEnded = true;
		//		};

		//		softphoneInstance.Connect();

		//		Task.Delay(ConnectionDelay).Wait();

		//		softphoneInstance.MakeCallAndRecord(ExampleURI, outFile, recordStartInstantly: true);

		//		Assert.AreEqual(lindotnet.Classes.LineState.Busy, softphoneInstance.LineState);
		//	}
		//	finally
		//	{
		//		while (!isCallEnded)
		//		{
		//			Thread.Sleep(100);
		//		}
		//		softphoneInstance.Disconnect();
		//	}
		//}

		[TestMethod]
		public void TestCallToAnotherSoftphone()
		{
			Softphone softphoneInstance = null;
			Call currentCall = null;
			try
			{
				// using office sip for testing
				var testAccount = new Account(
					login: "test",
					password: "testpass",
					host: "192.168.156.2",
					accountName: "TestUser");
				
				softphoneInstance = new Softphone(testAccount);

				softphoneInstance.CallActiveEvent += (call) =>
				{
					Task.Delay(ConnectionDelay).Wait();
					softphoneInstance.TerminateCall(call);
					Assert.IsNotNull(call);					
				};

				softphoneInstance.Connect();

				Task.Delay(ConnectionDelay).Wait();

				softphoneInstance.MakeCall(ExampleURI);

				Task.Delay(CallTime).Wait();
			}
			finally
			{
				softphoneInstance.Disconnect();
			}
		}
	}
}
