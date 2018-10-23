using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LoggingAPI;
using System.IO;
using System.Threading.Tasks;

namespace TestProject
{
	[TestClass]
	public class TestLogger
	{
		private static readonly TimeSpan WaitTime = TimeSpan.FromSeconds(5);

		private static readonly string workingDir = Environment.CurrentDirectory + @"\logs\";

		private static string LogFileName(DateTime dateTime)
		{
			return dateTime.ToString("yyyy-MM-dd");
		}

		[TestMethod]
		public void TestLogInfo()
		{
			string mock = "info: mock example bla bla bla";

			string fileName = LogFileName(DateTime.Now) + ".log";

			Logger.Info(mock);

			string path = workingDir + fileName;

			Task.Delay(WaitTime).Wait();

			if (File.Exists(path))
			{
				Assert.IsTrue(File.ReadAllText(path).Contains(mock));
			}
			else
			{
				throw new FileNotFoundException("Log file not created!");
			}
		}

		[TestMethod]
		public void TestLogError()
		{
			string mock = "error: mock example bla bla bla";

			string fileName = LogFileName(DateTime.Now) + ".log";

			Logger.Error(mock, new Exception(mock), Level.Fatal);

			string path = workingDir + fileName;

			Task.Delay(WaitTime).Wait();

			if (File.Exists(path))
			{
				Assert.IsTrue(File.ReadAllText(path).Contains(mock));
			}
			else
			{
				throw new FileNotFoundException("Log file not created!");
			}
		}
	}
}
