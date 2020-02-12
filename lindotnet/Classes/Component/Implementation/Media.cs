using lindotnet.Classes.Component.Interfaces;
using lindotnet.Classes.Helpers;
using lindotnet.Classes.Wrapper.Implementation.Modules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace lindotnet.Classes.Component.Implementation
{
    public class Media : IMedia
    {
        #region Fields

        private Softphone softphone;

        private double speakerGain;

        private bool microphoneEnable;

        private bool echoCancellation;

        private Device playbackDevice;

        private Device audioCaptureDevice;

        private Device videoCaptureDevice;

        #endregion Fields

        #region Props

        public Device PlaybackDevice
        {
            get
            {
                if (playbackDevice == null)
                {
                    IntPtr device = MediaModule.linphone_core_get_playback_device(softphone.LinphoneWrapper.LinphoneCore);
                    string deviceName = null;
                    if (MarshalingExtensions.TryConvert(device, out deviceName))
                    {
                        playbackDevice = new Device(deviceName)
                        {
                            IsActive = true
                        };
                    }
                }
                return playbackDevice;
            }
            set
            {
                if (value.IsActive)
                {
                    return;
                }
                if (value.Type != DeviceType.Playback)
                {
                    throw new LinphoneException("Device must be Playback!");
                }
                if (IsPlaybackDevice(value.ID))
                {
                    int code = MediaModule.linphone_core_set_playback_device(softphone.LinphoneWrapper.LinphoneCore, value.ID);
                    if (code == Constants.BOOL_T_FAILED_CODE)
                    {
                        throw new LinphoneException($"Device: {value} cannot be set!");
                    }
                    playbackDevice = value;
                }
            }
        }

        public Device AudioCaptureDevice
        {
            get
            {
                if (audioCaptureDevice == null)
                {
                    var devicePtr = MediaModule.linphone_core_get_capture_device(softphone.LinphoneWrapper.LinphoneCore);
                    string deviceName = null;
                    if (MarshalingExtensions.TryConvert(devicePtr, out deviceName))
                    {
                        audioCaptureDevice = new Device(deviceName)
                        {
                            IsActive = true
                        };
                    }
                }
                return audioCaptureDevice;
            }
            set
            {
                if (value.IsActive)
                {
                    return;
                }
                if (value.Type != DeviceType.SoundCapture)
                {
                    throw new LinphoneException("Device must be SoundCapture!");
                }
                if (IsAudioCaptureDevice(value.ID))
                {
                    int code = MediaModule.linphone_core_set_capture_device(softphone.LinphoneWrapper.LinphoneCore, value.ID);
                    if (code == Constants.BOOL_T_FAILED_CODE)
                    {
                        throw new LinphoneException($"Device: {value} cannot be set!");
                    }
                    audioCaptureDevice = value;
                }
            }
        }

        public Device VideoCaptureDevice
        {
            get
            {
                if (videoCaptureDevice == null)
                {
                    var devicePtr = MediaModule.linphone_core_get_video_device(softphone.LinphoneWrapper.LinphoneCore);
                    string deviceName = null;
                    if (MarshalingExtensions.TryConvert(devicePtr, out deviceName))
                    {
                        videoCaptureDevice = new Device(deviceName)
                        {
                            IsActive = true
                        };
                    }
                }
                return videoCaptureDevice;
            }
            set
            {
                if (value.IsActive)
                {
                    return;
                }
                if (value.Type != DeviceType.VideoCapture)
                {
                    throw new LinphoneException("Device must be SoundCapture!");
                }
                int code = MediaModule.linphone_core_set_video_device(softphone.LinphoneWrapper.LinphoneCore, value.ID);
                if (code == Constants.BOOL_T_FAILED_CODE)
                {
                    throw new LinphoneException($"Device: {value} cannot be set!");
                }
                videoCaptureDevice = value;
            }
        }

        /// <summary>
        /// Set or Get speaker gain value [0..1f]
        /// </summary>
        public double SpeakerGain
        {
            get
            {
                if (softphone.ActiveCall?.LinphoneCallPtr != null)
                {
                    speakerGain = MediaModule.linphone_call_get_speaker_volume_gain(softphone.ActiveCall.LinphoneCallPtr);
                }
                return speakerGain;
            }
            set
            {
                speakerGain = Math.Abs(value);
                if (softphone.ActiveCall?.LinphoneCallPtr != null)
                {
                    MediaModule.linphone_call_set_speaker_volume_gain(softphone.ActiveCall.LinphoneCallPtr, (float)speakerGain);
                }
            }
        }

        public bool MicrophoneEnable
        {
            get
            {
                microphoneEnable = MediaModule.linphone_core_mic_enabled(softphone.LinphoneWrapper.LinphoneCore);
                return microphoneEnable;
            }
            set
            {
                if (AudioCaptureDevice.IsActive)
                {
                    microphoneEnable = value;
                    MediaModule.linphone_core_enable_mic(softphone.LinphoneWrapper.LinphoneCore, microphoneEnable);
                }
            }
        }

        public bool EchoCancellation
        {
            get
            {
                if (softphone.ActiveCall?.LinphoneCallPtr != null)
                {
                    echoCancellation = MediaModule.linphone_call_echo_cancellation_enabled(softphone.ActiveCall.LinphoneCallPtr);
                }
                return echoCancellation;
            }
            set
            {
                if (softphone.ActiveCall?.LinphoneCallPtr != null)
                {
                    echoCancellation = value;
                    MediaModule.linphone_call_enable_echo_cancellation(softphone.ActiveCall.LinphoneCallPtr, echoCancellation);
                }
            }
        }

        #endregion Props

        public Media(Softphone softphone)
        {
            this.softphone = softphone;
            if (softphone.LinphoneWrapper == null)
            {
                throw new LinphoneException("LinphoneWrapper not created!");
            }
        }

        #region Methods

        public bool IsPlaybackDevice(string device)
        {
            return MediaModule.linphone_core_sound_device_can_playback(softphone.LinphoneWrapper.LinphoneCore, device);
        }

        public bool IsAudioCaptureDevice(string device)
        {
            return MediaModule.linphone_core_sound_device_can_capture(softphone.LinphoneWrapper.LinphoneCore, device);
        }

        public IEnumerable<Device> GetVideoCaptureDevices()
        {
            MediaModule.linphone_core_reload_video_devices(softphone.LinphoneWrapper.LinphoneCore);
            var videoDevsPtr = MediaModule.linphone_core_get_video_devices(softphone.LinphoneWrapper.LinphoneCore);

            return videoDevsPtr
                .ToStringCollection()
                .Select(devID => new Device(devID, DeviceType.VideoCapture));
        }

        public IEnumerable<Device> GetSoundDevices()
        {
            MediaModule.linphone_core_reload_sound_devices(softphone.LinphoneWrapper.LinphoneCore);
            var soundDevsPtr = MediaModule.linphone_core_get_sound_devices(softphone.LinphoneWrapper.LinphoneCore);

            return soundDevsPtr
                .ToStringCollection()
                .Select(device => new Device(device, IsAudioCaptureDevice(device) ? DeviceType.SoundCapture : DeviceType.Playback));
        }

        public void ReloadDevices()
        {
            MediaModule.linphone_core_reload_sound_devices(softphone.LinphoneWrapper.LinphoneCore);
            MediaModule.linphone_core_reload_video_devices(softphone.LinphoneWrapper.LinphoneCore);
        }

        /// <summary>
        /// Use property VideoCaptureDevice instead this method!
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        [Obsolete]
        public bool TrySetPlaybackDevice(string deviceId)
        {
            bool result = false;
            if (!string.IsNullOrWhiteSpace(deviceId) && MediaModule.linphone_core_sound_device_can_playback(softphone.LinphoneWrapper.LinphoneCore, deviceId))
            {
                int code = MediaModule.linphone_core_set_playback_device(softphone.LinphoneWrapper.LinphoneCore, deviceId);
                result = code != Constants.BOOL_T_FAILED_CODE;
            }
            return result;
        }

        #endregion Methods
    }
}