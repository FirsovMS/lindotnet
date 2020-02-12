using System;
using System.Runtime.InteropServices;

namespace lindotnet.Classes.Wrapper.Implementation.Loader
{
    internal static class LoadWindowsDlls
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string fileName);

        [DllImport("kernel32.dll")]
        public static extern int FreeLibrary(IntPtr handle);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr handle, string procedureName);

        [DllImport("msvcrt.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int vsprintf(IntPtr buffer, string format, IntPtr args);

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vscprintf(string format, IntPtr args);
    }
}