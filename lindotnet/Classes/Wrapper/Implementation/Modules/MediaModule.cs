using System;
using System.Runtime.InteropServices;
using static lindotnet.Classes.Wrapper.Implementation.Loader.Structs;

namespace lindotnet.Classes.Wrapper.Implementation.Modules
{
	/// <summary>
	/// http://www.linphone.org/docs/liblinphone/group__media__parameters.html
	/// </summary>
	internal static class MediaModule
	{
		/// <summary>
		/// Sets a wav file to be played when putting somebody on hold, or when files are used instead of soundcards 
		/// </summary>
		/// <param name="lc"></param>
		/// <param name="file">The file must be a 16 bit linear wav file.</param>
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
		public static extern void linphone_call_enable_echo_cancellation(IntPtr call, bool enable);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool linphone_call_echo_cancellation_enabled(IntPtr call);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void linphone_call_set_speaker_volume_gain(IntPtr call, float volume);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern float linphone_call_get_speaker_volume_gain(IntPtr call);

		#region Video

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void linphone_core_reload_video_devices(IntPtr lc);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void linphone_core_enable_video_capture(IntPtr lc, bool enable);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern int linphone_core_set_video_device(IntPtr lc, string device);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr linphone_core_get_video_device(IntPtr lc);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void linphone_core_enable_video_display(IntPtr lc, bool enable);

		/// <summary>
		/// Use to enable multicast rtp for video stream. If enabled, outgoing calls put a multicast address from 
		/// linphone_core_get_video_multicast_addr into video cline. 
		/// In case of outgoing call video stream is sent to this multicast address. For incoming calls behavior is unchanged.
		/// </summary>
		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void linphone_core_enable_video_multicast(IntPtr lc, bool yesno);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void linphone_core_enable_video_preview(IntPtr lc, bool val);

		/// <summary>
		/// Set the preferred frame rate for video. Based on the available bandwidth constraints and network conditions,
		/// the video encoder remains free to lower the framerate. There is no warranty that the preferred frame rate be
		/// the actual framerate. used during a call. Default value is 0, which means "use encoder's default fps value".
		/// </summary>
		/// <param name="fps"></param>
		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void linphone_core_set_preferred_framerate(IntPtr lc, float fps);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern float linphone_core_get_preferred_framerate(IntPtr lc);

		/// <summary>
		/// Sets the preferred video size.
		/// </summary>
		/// <param name="lc"></param>
		/// <param name="vsize">MSVideoSize* vsize</param>
		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void linphone_core_set_preferred_video_size(IntPtr lc, IntPtr vsize);

		/// <returns>MSVideoSize *</returns>
		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr linphone_core_get_preferred_video_size(IntPtr lc);

		/// <summary>
		/// Sets the video size for the captured (preview) video
		/// </summary>
		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr linphone_core_set_preview_video_size(IntPtr factory);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern int linphone_core_set_static_picture(IntPtr lc, string path);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern string linphone_core_get_static_picture(IntPtr lc);

		#endregion

		#region Encryption

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern int linphone_core_set_media_encryption(IntPtr lc, LinphoneMediaEncryption menc);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern LinphoneMediaEncryption linphone_core_get_media_encryption(IntPtr lc);

		#endregion

		#region Codecs

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern BctbxList linphone_core_get_audio_codecs(IntPtr lc);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern int linphone_core_set_audio_codecs(IntPtr lc, BctbxList list);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern BctbxList linphone_core_get_video_codecs(IntPtr lc);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern int linphone_core_set_video_codecs(IntPtr lc, BctbxList list);

		#endregion
	}
}
