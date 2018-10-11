using System;
using System.Runtime.InteropServices;

namespace lindotnet.Classes.Wrapper.Implementation.Modules
{
	internal static class GenericModules
	{
		/// <summary>
		/// http://www.linphone.org/docs/liblinphone/group__linphone__address.html
		/// </summary>
		/// <param name="u"></param>
		#region SIP

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		[System.Obsolete]
		public static extern void linphone_address_destroy(IntPtr u);

		#endregion

		#region Miscenalleous

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void linphone_core_set_user_agent(IntPtr lc, string ua_name, string version);

		#endregion


		/// <summary>
		/// http://www.linphone.org/docs/liblinphone/group__authentication.html
		/// </summary>
		/// <param name="lc"></param>
		/// <param name="info"></param>
		#region Authentication

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void linphone_core_add_auth_info(IntPtr lc, IntPtr info);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr linphone_auth_info_new(string username, string userid, string passwd, string ha1, string realm, string domain);

		#endregion

		/// <summary>
		/// http://www.linphone.org/docs/liblinphone/group__call__misc.html
		/// </summary>
		/// <param name="call"></param>
		#region Calls miscenalleous

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void linphone_call_start_recording(IntPtr call);

		[DllImport(Constants.LIBNAME, CallingConvention = CallingConvention.Cdecl)]
		public static extern void linphone_call_stop_recording(IntPtr call);

		#endregion
	}
}
