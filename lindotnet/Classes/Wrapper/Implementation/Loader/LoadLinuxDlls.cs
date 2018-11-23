using System;
using System.Runtime.InteropServices;

namespace lindotnet.Classes.Wrapper.Implementation.Loader
{
	internal static class LoadLinuxDlls
	{
		[DllImport("libdl.so")]
		public static extern IntPtr dlopen(String fileName, int flags);

		[DllImport("libdl.so")]
		public static extern IntPtr dlsym(IntPtr handle, String symbol);

		[DllImport("libdl.so")]
		public static extern int dlclose(IntPtr handle);

		[DllImport("libdl.so")]
		public static extern IntPtr dlerror();

		[DllImport("libc", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern int vsprintf(IntPtr buffer, [In][MarshalAs(UnmanagedType.LPStr)] string format, IntPtr args);

		[DllImport("libc", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern int vsnprintf(IntPtr buffer, UIntPtr size, [In][MarshalAs(UnmanagedType.LPStr)] string format, IntPtr args);
	}
}
