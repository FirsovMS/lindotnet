using System;

namespace LoggingAPI.Data
{
	[Serializable]
	public class ExceptionMessage
	{
		public string Message { get; set; }

		public string Source { get; set; }

		public string StackTrace { get; set; }
	}
}