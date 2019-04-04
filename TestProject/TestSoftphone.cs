using lindotnet.Classes;
using lindotnet.Classes.Component.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace TestProject
{
	[TestClass]
	public class TestSoftphone
	{
		private static readonly string workingDir = Environment.CurrentDirectory + @"\records";
		private static readonly string ExampleURI = "test2";
		private Softphone _softphoneInstance;

		[TestInitialize]
		public void BeforeTestInitialize()
		{
			// using office sip for testing
			var testAccount = new Account(
				login: "test",
				password: "testpass",
				host: "officesip.local",
				accountName: "TestUser");

			_softphoneInstance = new Softphone(testAccount);
		}

		[TestMethod]
		public void CreateSoftphoneInstance()
		{
			Assert.IsNotNull(_softphoneInstance);
		}

		[TestMethod]
		public void TestSoftphoneServerConnection()
		{
			_softphoneInstance.Connect();

			Assert.IsTrue(_softphoneInstance.ConnectionState == ConnectState.Connected);
		}

		[TestMethod]
		public void TestCallToAnotherSoftphone()
		{
			_softphoneInstance.CallActiveEvent += (call) =>
			{
				_softphoneInstance.TerminateCall(call);
				Assert.IsNotNull(call);
			};

			_softphoneInstance.PhoneConnectedEvent += () =>
			{
				_softphoneInstance.MakeCall(ExampleURI);
			};

			_softphoneInstance.Connect();
		}

		[TestCleanup]
		public void AfterTestComplete()
		{
			_softphoneInstance?.Disconnect();
			_softphoneInstance = null;
		}
	}
}
