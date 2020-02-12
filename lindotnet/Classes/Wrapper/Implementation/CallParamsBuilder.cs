using lindotnet.Classes.Wrapper.Implementation.Modules;
using lindotnet.Classes.Wrapper.Interfaces;
using System;

namespace lindotnet.Classes.Wrapper.Implementation
{
    public class CallParamsBuilder : ICallParamsBuilder
    {
        private IntPtr linphoneCore;
        private IntPtr callParams;

        public CallParamsBuilder(IntPtr linphoneCore)
        {
            this.linphoneCore = linphoneCore;
        }

        public override IntPtr Build()
        {
            return callParams;
        }

        public override CallParamsBuilder BuildAudioParams(bool enableAudio = true)
        {
            callParams = CallModule.linphone_core_create_call_params(linphoneCore, IntPtr.Zero);
            callParams = CallModule.linphone_call_params_ref(callParams);

            CallModule.linphone_call_params_enable_audio(callParams, enableAudio);

            return this;
        }

        public override CallParamsBuilder BuildMediaParams(bool enablEarlyMediaSending = true)
        {
            CallModule.linphone_call_params_enable_early_media_sending(callParams, enablEarlyMediaSending);

            return this;
        }

        public override CallParamsBuilder BuildVideoParams(bool enableVideo = false)
        {
            CallModule.linphone_call_params_enable_video(callParams, enableVideo);

            return this;
        }
    }
}