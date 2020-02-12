using System;
using System.Runtime.InteropServices;

namespace lindotnet.Classes.Wrapper.Implementation.Modules
{
    /// <summary>
    /// http://www.linphone.org/docs/liblinphone/group__initializing.html
    /// </summary>
    internal static class CoreModule
    {
        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        [Obsolete]
        public static extern IntPtr linphone_core_new(IntPtr vtable, string config_path, string factory_config_path, IntPtr userdata);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void linphone_core_unref(IntPtr lc);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void linphone_core_iterate(IntPtr lc);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void linphone_core_set_log_level(OrtpLogLevel loglevel);

        [DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void linphone_core_set_log_handler(IntPtr logfunc);
    }
}