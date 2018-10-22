using System;

namespace lindotnet.Classes.Component.Implementation
{
	public class Device
	{
		private string id;

		public string ID
		{
			get
			{
				if (string.IsNullOrWhiteSpace(id))
				{
					throw new ArgumentException("Device id must be set!");
				}
				return id;
			}
			private set
			{
				if (string.IsNullOrWhiteSpace(id))
				{
					throw new ArgumentException("Device id can be null, empty or whitespace!");
				}
				id = value;
			}
		}

		public DeviceType Type { get; private set; }

		public bool IsActive { get; internal set; }

		public Device(string deviceId, DeviceType deviceType = DeviceType.Playback)
		{
			ID = deviceId;
			Type = deviceType;
		}

		public override string ToString()
		{
			return $"ID: {ID}, Type: {Type}, Active: {IsActive}";
		}
	}
}