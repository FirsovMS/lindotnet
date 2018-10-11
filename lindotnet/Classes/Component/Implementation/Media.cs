using System;
using System.Collections.Generic;
using lindotnet.Classes.Component.Interfaces;
using lindotnet.Classes.Wrapper.Implementation;

namespace lindotnet.Classes.Component.Implementation
{
	internal class Media : IMedia
	{
		private LinphoneWrapper linphoneWrapper;

		public Media(LinphoneWrapper linphoneWrapper)
		{
			this.linphoneWrapper = linphoneWrapper;
		}

		public List<string> CaptureDevices()
		{
			throw new NotImplementedException();
		}

		public void EchoCancellation(Call call, bool val)
		{
			throw new NotImplementedException();
		}

		public double GetSpeakerValue(Call call)
		{
			throw new NotImplementedException();
		}

		public List<string> PlaybackDevices()
		{
			throw new NotImplementedException();
		}

		public void SetSpeakerValue(Call call, float value)
		{
			throw new NotImplementedException();
		}
	}
}
