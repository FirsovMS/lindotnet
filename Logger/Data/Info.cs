using System;

namespace LoggingAPI.Data
{
	[Serializable]
	public class Info
	{
		public string Message { get; set; }

		public DateTime Date { get; set; }
	}
}
