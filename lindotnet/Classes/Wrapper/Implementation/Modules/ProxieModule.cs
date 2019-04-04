using System;
using System.Runtime.InteropServices;

namespace lindotnet.Classes.Wrapper.Implementation.Modules
{
	/// <summary>
	/// http://www.linphone.org/docs/liblinphone/group__proxies.html
	/// </summary>
	internal class ProxieModule
	{
		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr linphone_core_create_proxy_config(IntPtr lc);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern int linphone_proxy_config_set_identity(IntPtr obj, string identity);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern int linphone_proxy_config_set_server_addr(IntPtr obj, string server_addr);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void linphone_proxy_config_enable_register(IntPtr obj, bool val);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern int linphone_core_add_proxy_config(IntPtr lc, IntPtr cfg);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void linphone_core_set_default_proxy_config(IntPtr lc, IntPtr config);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		[System.Obsolete]
		public static extern int linphone_core_get_default_proxy(IntPtr lc, ref IntPtr config);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool linphone_proxy_config_is_registered(IntPtr config);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void linphone_proxy_config_edit(IntPtr config);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern int linphone_proxy_config_done(IntPtr config);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void linphone_proxy_config_set_nat_policy(IntPtr cfg, IntPtr policy);
	}
}
