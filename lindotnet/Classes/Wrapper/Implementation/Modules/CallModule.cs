using System;
using System.Runtime.InteropServices;

namespace lindotnet.Classes.Wrapper.Implementation.Modules
{
    /// <summary>
    /// http://www.linphone.org/docs/liblinphone/group__call__control.html
    /// </summary>
    internal static class CallModule
    {
        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr linphone_core_create_call_params(IntPtr lc, IntPtr call);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void linphone_call_params_enable_video(IntPtr lc, bool enabled);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void linphone_call_params_enable_early_media_sending(IntPtr lc, bool enabled);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr linphone_core_invite_with_params(IntPtr lc, string url, IntPtr callparams);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr linphone_call_get_params(IntPtr call);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr linphone_call_params_ref(IntPtr callparams);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void linphone_call_params_unref(IntPtr callparams);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int linphone_core_terminate_call(IntPtr lc, IntPtr call);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr linphone_call_ref(IntPtr call);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void linphone_call_unref(IntPtr call);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int linphone_core_terminate_all_calls(IntPtr lc);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr linphone_call_get_remote_address_as_string(IntPtr call);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr linphone_address_as_string(IntPtr address);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr linphone_address_as_string_uri_only(IntPtr address);

#warning DllNotLoad
        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr linphone_call_get_to_address(IntPtr call);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int linphone_core_accept_call_with_params(IntPtr lc, IntPtr call, IntPtr callparams);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void linphone_call_params_set_record_file(IntPtr callparams, string filename);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr linphone_call_params_get_record_file(IntPtr callparams);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void linphone_call_params_enable_audio(IntPtr callparams, bool enabled);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int linphone_call_send_dtmfs(IntPtr call, string dtmfs);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int linphone_core_pause_call(IntPtr lc, IntPtr call);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int linphone_core_resume_call(IntPtr lc, IntPtr call);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int linphone_call_redirect(IntPtr lc, IntPtr call, string redirect_uri);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int linphone_call_transfer(IntPtr lc, IntPtr call, string redirect_uri);
    }
}