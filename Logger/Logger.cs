using Addititonals.Helpers;
using LoggingAPI.Data;
using NLog;
using System;
using System.Collections.Generic;

namespace LoggingAPI
{
	public static class Logger
	{
		private static NLog.Logger logger;

		static Logger()
		{
			logger = LogManager.GetCurrentClassLogger();
		}

		public static void Info(string description)
		{
			var info = new Info()
			{
				Message = description,
				Date = DateTime.Now
			};
			logger.Info(info.SerializeObject());
		}

		public static void Error(string description, Exception exception = null, Level logLevel = Level.Debug, string sqlQuery = null)
		{
			var message = new ErrorMessage()
			{
				Error = new ErrorDescription()
				{
					Description = description,
					Message = CreateExceptionMessages(exception)
				},
				sql = sqlQuery,
				Date = DateTime.Now,
				LogLevel = logLevel
			};
			LogError(message);
		}

		private static List<ExceptionMessage> CreateExceptionMessages(Exception exception)
		{
			var result = new List<ExceptionMessage>();
			result.Add(CreateExceptionMessage(exception));

			var innerException = exception.InnerException;
			while (innerException != null)
			{
				result.Add(CreateExceptionMessage(exception));
				innerException = innerException.InnerException;
			}

			return result;
		}

		private static ExceptionMessage CreateExceptionMessage(Exception exception)
		{
			return new ExceptionMessage
			{
				Message = exception.Message,
				Source = exception.Source,
				StackTrace = exception.StackTrace
			};
		}


		private static void LogError(ErrorMessage message)
		{
			switch (message.LogLevel)
			{
				case Level.Trace:
					logger.Trace(message.SerializeObject());
					break;
				case Level.Debug:
					logger.Debug(message.SerializeObject());
					break;
				case Level.Warn:
					logger.Warn(message.SerializeObject());
					break;
				case Level.Error:
					logger.Error(message.SerializeObject());
					break;
				case Level.Fatal:
					logger.Fatal(message.SerializeObject());
					break;
			}
		}
	}
}
