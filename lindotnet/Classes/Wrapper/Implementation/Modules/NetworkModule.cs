using System;
using System.Runtime.InteropServices;

namespace lindotnet.Classes.Wrapper.Implementation.Modules
{
	/// <summary>
	/// http://www.linphone.org/docs/liblinphone/group__network__parameters.html
	/// </summary>
	internal static class NetworkModule
	{
		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern int linphone_core_set_sip_transports(IntPtr lc, IntPtr tr_config);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr linphone_core_create_nat_policy(IntPtr lc);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void linphone_nat_policy_unref(IntPtr natpolicy);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr linphone_nat_policy_ref(IntPtr natpolicy);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void linphone_nat_policy_clear(IntPtr policy);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void linphone_nat_policy_enable_stun(IntPtr policy, bool enable);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void linphone_nat_policy_enable_turn(IntPtr policy, bool enable);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void linphone_nat_policy_enable_ice(IntPtr policy, bool enable);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void linphone_nat_policy_enable_upnp(IntPtr policy, bool enable);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void linphone_nat_policy_set_stun_server(IntPtr policy, string stun_server);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void linphone_nat_policy_set_stun_server_username(IntPtr policy, string username);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void linphone_nat_policy_resolve_stun_server(IntPtr policy);
	}
}
