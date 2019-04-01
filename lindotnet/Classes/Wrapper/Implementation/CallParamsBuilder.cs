using System;
using lindotnet.Classes.Wrapper.Implementation.Modules;
using lindotnet.Classes.Wrapper.Interfaces;

namespace lindotnet.Classes.Wrapper.Implementation
{
	public class CallParamsBuilder : ICallParamsBuilder
	{
		private readonly IntPtr _linphoneCore;
		private IntPtr _callParams;

		public CallParamsBuilder(IntPtr linphoneCore)
		{
			_linphoneCore = linphoneCore;
		}

		public override IntPtr Build()
		{
			return _callParams;
		}

		public override CallParamsBuilder BuildAudioParams(bool enableAudio = true)
		{
			_callParams = CallModule.linphone_core_create_call_params(_linphoneCore, IntPtr.Zero);
			_callParams = CallModule.linphone_call_params_ref(_callParams);

			CallModule.linphone_call_params_enable_audio(_callParams, enableAudio);

			return this;
		}

		public override CallParamsBuilder BuildMediaParams(bool enablEarlyMediaSending = true)
		{
			CallModule.linphone_call_params_enable_early_media_sending(_callParams, enablEarlyMediaSending);

			return this;
		}

		public override CallParamsBuilder BuildVideoParams(bool enableVideo = false)
		{
			CallModule.linphone_call_params_enable_video(_callParams, enableVideo);

			return this;
		}
	}
}
