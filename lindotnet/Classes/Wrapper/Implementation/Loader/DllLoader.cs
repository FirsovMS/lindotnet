using System;
using System.Runtime.InteropServices;

namespace lindotnet.Classes.Wrapper.Implementation.Loader
{
	/// <summary>
	/// Cross-platform native loading utils.
	/// By http://dimitry-i.blogspot.ru/2013/01/mononet-how-to-dynamically-load-native.html
	/// </summary>
	internal static class DllLoader
	{
		public static IntPtr DoLoadLibrary(string fileName)
		{
			return LoadWindowsDlls.LoadLibrary(fileName);
		}

		public static void DoFreeLibrary(IntPtr handle)
		{
			LoadWindowsDlls.FreeLibrary(handle);
		}

		public static IntPtr DoGetProcAddress(IntPtr dllHandle, string name)
		{
			return LoadWindowsDlls.GetProcAddress(dllHandle, name);
		}

		public static string ProcessVAlist(string format, IntPtr args)
		{
			return ProcessVAListOnWindows(format, args);
		}

		private static string ProcessVAListOnWindows(string format, IntPtr args)
		{
			int byteLength = LoadWindowsDlls.vscprintf(format, args) + 1;
			IntPtr buffer = Marshal.AllocHGlobal(byteLength);

			LoadWindowsDlls.vsprintf(buffer, format, args);
			return Marshal.PtrToStringAnsi(buffer);
		}
	}
}
