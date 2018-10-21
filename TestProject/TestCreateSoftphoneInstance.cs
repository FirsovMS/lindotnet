using lindotnet.Classes.Component.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
    [TestClass]
    public class TestCreateSoftphoneInstance
    {
        [TestMethod]
        public void SoftphoneInstanceCreate()
        {
            // using office sip for testing
            var testAccount = new Account(
                login: "test",
                password: "testpass",
                host: "officesip.local",
                accountName: "TestUser");

            Softphone softphoneInstance = new Softphone(testAccount);
        }
    }
}
