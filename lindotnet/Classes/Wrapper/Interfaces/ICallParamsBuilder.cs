using lindotnet.Classes.Wrapper.Implementation;
using System;

namespace lindotnet.Classes.Wrapper.Interfaces
{
    public abstract class ICallParamsBuilder
    {
        public abstract CallParamsBuilder BuildAudioParams(bool enableAudio = true);

        public abstract CallParamsBuilder BuildVideoParams(bool enableVideo = false);

        public abstract CallParamsBuilder BuildMediaParams(bool enablEarlyMediaSending = true);

        public abstract IntPtr Build();
    }
}