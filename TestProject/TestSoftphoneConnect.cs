using lindotnet.Classes.Component.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using Assert = NUnit.Framework.Assert;

namespace TestProject
{
    [TestClass]
    public class TestSoftphoneConnect
    {
        public Account testAccount { get; private set; }
        public Softphone softphoneInstance { get; private set; }

        [SetUp]
        public void CreateInstances()
        {
            // using office sip for testing
            testAccount = new Account(
                login: "test",
                password: "testpass",
                host: "officesip.local",
                accountName: "TestUser");

            softphoneInstance = new Softphone(testAccount);
        }

        [TestMethod]
        public void TestConnect()
        {
            try
            {
                softphoneInstance.Connect();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
            
        }

        [TearDown]
        public void DestroyInstances()
        {
            Assert.Fail("Not implemented!");
        }
    }
}
