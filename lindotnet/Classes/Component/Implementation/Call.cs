namespace lindotnet.Classes.Component.Implementation
{
	public class Call
	{
		public CallType Type { get; protected set; }

		public CallState State { get; protected set; }

		public string CallTo { get; protected set; }

		public string CallFrom { get; protected set; }

		public string RecordFile { get; protected set; }
	}
}
