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
        public static IntPtr LoadLibrary(string fileName)
        {
            return LoadDlls.LoadLibrary(fileName);
        }

        public static void FreeLibrary(IntPtr handle)
        {
            LoadDlls.FreeLibrary(handle);
        }

        public static IntPtr GetProcAddress(IntPtr dllHandle, string name)
        {
            return LoadDlls.GetProcAddress(dllHandle, name);
        }

        private static string GetVirtualAdress(string format, IntPtr args)
        {
            int byteLength = LoadDlls.vscprintf(format, args) + 1;
            IntPtr buffer = Marshal.AllocHGlobal(byteLength);

            try
            {
                LoadDlls.vsprintf(buffer, format, args);

                return Marshal.PtrToStringAnsi(buffer);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
    }
}