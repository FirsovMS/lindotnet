using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingAPI
{
	public static class Logger
	{
		private static Lazy<NLog.Logger> logger;

		static Logger()
		{
			logger = new Lazy<NLog.Logger>(() =>
					   LogManager.GetCurrentClassLogger()
			, true);
		}

		public static void Info(string message)
		{
			logger.Value.Info(message);
		}

		public static void Error(string message, Exception exception = null, Level warningLevel = Level.Debug)
		{
			switch (warningLevel)
			{
				case Level.Trace:
					logger.Value.Trace(exception, message);
					break;
				case Level.Debug:
					logger.Value.Debug(message);
					break;
				case Level.Info:
					logger.Value.Info(message);
					break;
				case Level.Warn:
					logger.Value.Warn(message);
					break;
				case Level.Error:
					logger.Value.Error(exception, message);
					break;
				case Level.Fatal:
					logger.Value.Fatal(exception, message);
					break;
			}
		}
	}
}
