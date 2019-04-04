using System;
using System.Runtime.InteropServices;

namespace lindotnet.Classes.Wrapper.Implementation.Modules
{
	/// <summary>
	/// http://www.linphone.org/docs/liblinphone/group__initializing.html
	/// </summary>
	internal class CoreModule
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

		/// <summary>
		/// The LinphoneCore object is the primary handle for doing all phone actions. It should be unique within your application.
		/// </summary>
		/// <param name="linphoneFactory">The LinphoneFactory singleton.</param>
		/// <param name="linphoneCoreCbs">a LinphoneCoreCbs object holding your application callbacks. A reference will be taken on it until the destruciton of the core 
		/// or the unregistration with linphone_core_remove_cbs()</param>
		/// <param name="configPath">a path to a config file. If it does not exists it will be created. The config file is used to store all settings, call logs, 
		/// friends, proxies... so that all these settings become persistent over the life of the LinphoneCore object.
		/// It is allowed to set a NULL config file. In that case LinphoneCore will not store any settings.</param>
		/// <param name="factoryConfigPath">a path to a read-only config file that can be used to to store hard-coded preference such as proxy settings or internal preferences.
		/// The settings in this factory file always override the one in the normal config file. It is OPTIONAL, use NULL if unneeded.</param>
		/// <returns></returns>
		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr linphone_factory_create_core(IntPtr linphoneFactory, IntPtr linphoneCoreCbs, string configPath = null, string factoryConfigPath = null);

		/// <summary>
		/// Create the LinphoneFactory if that has not been done and return a pointer on it.
		/// </summary>
		/// <returns> LinphoneFactory*</returns>
		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr linphone_factory_get();

		/// <summary>
		/// Clean the factory. This function is generally useless as the factory is unique per process, however calling
		/// this function at the end avoid getting reports from belle-sip leak detector about memory leaked in linphone_factory_get().
		/// </summary>
		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void linphone_factory_clean();

		/// <summary>
		/// Instanciate a LinphoneCoreCbs object.
		/// </summary>
		/// <param name="factory"></param>
		/// <returns></returns>
		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr linphone_factory_create_core_cbs(IntPtr factory);

		/// <summary>
		/// Instantiates a LinphoneCore object with a given LpConfig.
		/// </summary>
		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr linphone_factory_create_core_with_config(IntPtr factory, IntPtr cbs, string config_path);
	}
}
