using LoggingAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
	[TestClass]
	public class TestLogger
	{
		[TestMethod]
		public void TestLoggerInfo()
		{
			string mock = "dummy: testing logger info";

			Logger.Info(mock);


		}
	}
}
