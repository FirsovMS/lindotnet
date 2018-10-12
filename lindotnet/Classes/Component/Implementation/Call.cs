namespace lindotnet.Classes.Component.Implementation
{
	public class Call
	{
		public CallType Type { get; internal set; } = CallType.None;

		public CallState State { get; internal set; } = CallState.None;

		public string To { get; internal set; } = string.Empty;

		public string From { get; internal set; } = string.Empty;

		public string RecordFile { get; internal set; } = string.Empty;
	}
}
