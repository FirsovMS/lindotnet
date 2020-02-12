using System;
using System.Runtime.InteropServices;

namespace lindotnet.Classes.Wrapper.Implementation.Modules
{
    /// <summary>
    /// http://www.linphone.org/docs/liblinphone/group__media__parameters.html
    /// </summary>
    internal static class MediaModule
    {
        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void linphone_core_set_play_file(IntPtr lc, string file);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void linphone_core_set_record_file(IntPtr lc, string file);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void linphone_core_set_ring(IntPtr lc, string file);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void linphone_core_set_remote_ringback_tone(IntPtr lc, string file);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void linphone_core_set_ringback(IntPtr lc, string file);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void linphone_core_reload_sound_devices(IntPtr lc);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void linphone_core_reload_video_devices(IntPtr lc);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool linphone_core_sound_device_can_capture(IntPtr lc, string device);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool linphone_core_sound_device_can_playback(IntPtr lc, string device);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr linphone_core_get_ringer_device(IntPtr lc);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr linphone_core_get_playback_device(IntPtr lc);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr linphone_core_get_capture_device(IntPtr lc);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int linphone_core_set_ringer_device(IntPtr lc, string devid);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int linphone_core_set_playback_device(IntPtr lc, string devid);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int linphone_core_set_capture_device(IntPtr lc, string devid);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void linphone_core_enable_mic(IntPtr lc, bool enable);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool linphone_core_mic_enabled(IntPtr lc);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr linphone_core_get_sound_devices(IntPtr lc);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr linphone_core_get_video_devices(IntPtr lc);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void linphone_core_enable_video_capture(IntPtr lc, bool enable);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        [Obsolete]
        public static extern void linphone_core_enable_video_capture(IntPtr lc, bool vcap_enabled, bool display_enabled);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void linphone_call_enable_echo_cancellation(IntPtr call, bool enable);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool linphone_call_echo_cancellation_enabled(IntPtr call);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void linphone_core_enable_video_display(IntPtr lc, bool enable);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void linphone_core_enable_video_multicast(IntPtr lc, bool yesno);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void linphone_core_enable_video_preview(IntPtr lc, bool val);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void linphone_call_set_speaker_volume_gain(IntPtr call, float volume);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern float linphone_call_get_speaker_volume_gain(IntPtr call);

        #region Video

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int linphone_core_set_video_device(IntPtr lc, string device);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr linphone_core_get_video_device(IntPtr lc);

        #endregion Video
    }
}