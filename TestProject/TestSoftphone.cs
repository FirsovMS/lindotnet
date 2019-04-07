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
		private static readonly string test2 = "test2";
		private Softphone _softphoneInstance;

		[TestInitialize]
		public void BeforeTestInitialize()
		{
			var testAccount = new Account("test", "test", "local.dev", "localhost", accountName: "test");

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
				_softphoneInstance.MakeCall(test2);
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
