using System;
using System.Collections.Generic;

namespace LoggingAPI.Data
{
	[Serializable]
	public class ErrorDescription
	{
		public string Description { get; set; }

		public List<ExceptionMessage> Message { get; set; }
	}
}
