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
		[Obsolete]
		public static extern IntPtr linphone_core_new_with_config(IntPtr vtable, IntPtr config, IntPtr userdata);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void linphone_core_unref(IntPtr lc);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void linphone_core_iterate(IntPtr lc);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void linphone_core_set_log_level(OrtpLogLevel loglevel);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void linphone_core_set_log_handler(IntPtr logfunc);

		// Add a new core factories
		/// <summary>
		/// The LinphoneCore object is the primary handle for doing all phone actions. It should be unique within your application.
		/// </summary>
		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr linphone_factory_create_core(IntPtr factory, IntPtr cbs, string config_path, string factory_config_path);

		/// <summary>
		/// Instantiates a LinphoneCore object with a given LpConfig.
		/// </summary>
		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr linphone_factory_create_core_with_config(IntPtr factory, IntPtr cbs, string config_path);

		/// <summary>
		/// Instanciate a LinphoneCoreCbs object.
		/// </summary>
		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr linphone_factory_create_core_cbs(IntPtr factory);
	}
}
