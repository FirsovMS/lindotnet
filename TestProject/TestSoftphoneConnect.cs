using lindotnet;
using lindotnet.Classes.Component.Implementation;
using lindotnet.Classes.Component.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace TestProject
{
    [TestClass]
    public class TestSoftphoneConnect
    {
        public Account testAccount { get; private set; }
        public ISoftphone softphoneInstance { get; private set; }

        [SetUp]
        public void CreateInstances()
        {
            // using office sip for testing
            testAccount = new Account(
                login: "test",
                password: "testpass",
                host: "officesip.local",
                accountName: "TestUser");

            softphoneInstance = Container.CreateNewSoftphone(testAccount);
        }

        [TestMethod]
        public void TestConnect()
        {
            softphoneInstance.
        }

        [TearDown]
        public void DestroyInstances()
        {
            
        }
    }
}
