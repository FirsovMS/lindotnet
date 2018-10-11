using System.Collections.Generic;
using lindotnet.Classes.Component.Implementation;

namespace lindotnet.Classes.Component.Interfaces
{
	public interface IMedia
	{
		List<string> PlaybackDevices();

		List<string> CaptureDevices();

		double GetSpeakerValue(Call call);

		void SetSpeakerValue(Call call, float value);

		void EchoCancellation(Call call, bool val);
	}
}
