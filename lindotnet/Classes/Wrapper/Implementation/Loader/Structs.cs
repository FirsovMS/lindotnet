using System;
using System.Runtime.InteropServices;

namespace lindotnet.Classes.Wrapper.Implementation.Loader
{
    public class Structs
    {
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct VaListWindows
        {
            private IntPtr Pointer;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct VaListLinuxX64
        {
            private UInt32 gp_offset;
            private UInt32 fp_offset;
            private IntPtr overflow_arg_area;
            private IntPtr reg_save_area;
        }
    }
}