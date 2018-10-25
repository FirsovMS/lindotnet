using System.Collections.Generic;
using lindotnet.Classes.Component.Implementation;

namespace lindotnet.Classes.Component.Interfaces
{
	public interface IMedia
	{
		bool IsPlaybackDevice(string device);

		bool IsAudioCaptureDevice(string device);

		IEnumerable<Device> GetVideoCaptureDevices();

		IEnumerable<Device> GetSoundDevices();

		bool TrySetPlaybackDevice(string device_id);

		void ReloadDevices();
	}
}
