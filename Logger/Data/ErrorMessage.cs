using System;

namespace LoggingAPI.Data
{
	[Serializable]
	public class ErrorMessage
	{
		public DateTime Date { get; set; }

		public Level LogLevel { get; set; }

		public ErrorDescription Error { get; set; }

		public string sql { get; set; }
	}
}
