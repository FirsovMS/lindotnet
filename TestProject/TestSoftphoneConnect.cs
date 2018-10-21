using lindotnet.Classes.Component.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace TestProject
{
    [TestClass]
    public class TestSoftphoneConnect
    {
        private static readonly TimeSpan ConnectionDelay = TimeSpan.FromSeconds(5);

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
        public void TestConnect()
        {
            // using office sip for testing
            var testAccount = new Account(
                login: "test",
                password: "testpass",
                host: "officesip.local",
                accountName: "TestUser");

            var softphoneInstance = new Softphone(testAccount);

            softphoneInstance.Connect();

            Task.Delay(ConnectionDelay).Wait();

            Assert.AreEqual(lindotnet.Classes.ConnectState.Connected, softphoneInstance.ConnectState);
        }

        [TestMethod]
        public void TestCallToAnotherSoftphone()
        {
            try
            {
                // using office sip for testing
                var testAccount = new Account(
                    login: "test",
                    password: "testpass",
                    host: "officesip.local",
                    accountName: "TestUser");

                var softphoneInstance = new Softphone(testAccount);

                // using office sip for testing
                var anotherTestAccount = new Account(
                    login: "test2",
                    password: "testpass2",
                    host: "officesip.local",
                    accountName: "AnotherUser");

                var anotherSoftphoneInstance = new Softphone(anotherTestAccount);

                // subscribe on event
                anotherSoftphoneInstance.IncomingCallEvent += (Call call) =>
                {
                    Assert.AreEqual<string>(testAccount.Identiny, call.From);
                };

                // connect to server
                softphoneInstance.Connect();
                anotherSoftphoneInstance.Connect();

                Task.Delay(ConnectionDelay).Wait();

                // create call between two accounts
                softphoneInstance.MakeCall(anotherSoftphoneInstance.Account.Identiny);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}
