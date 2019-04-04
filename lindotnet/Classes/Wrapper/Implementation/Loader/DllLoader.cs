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
		public static IntPtr LoadLibrary(string path)
		{
			return LoadWindowsDlls.LoadLibrary(path);
		}

		public static void FreeLibrary(IntPtr libraryName)
		{
			LoadWindowsDlls.FreeLibrary(libraryName);
		}

		public static IntPtr ProcedureCall(IntPtr handleDLL, string name)
		{
			return LoadWindowsDlls.GetProcAddress(handleDLL, name);
		}

		public static string ProcessCharArrays(string format, IntPtr args)
		{
			return ProcessVAListOnWindows(format, args);
		}

		private static string ProcessVAListOnWindows(string format, IntPtr args)
		{
			var byteLength = LoadWindowsDlls.vscprintf(format, args) + 1;
			var buffer = Marshal.AllocHGlobal(byteLength);

			LoadWindowsDlls.vsprintf(buffer, format, args);
			return Marshal.PtrToStringAnsi(buffer);
		}
	}
}
