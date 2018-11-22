using System;

namespace LoggingAPI.Data
{
	[Serializable]
	public class ErrorDescription
	{
		public string Description { get; set; }

		public string Message { get; set; }

		public string StackTrace { get; set; }
	}
}
