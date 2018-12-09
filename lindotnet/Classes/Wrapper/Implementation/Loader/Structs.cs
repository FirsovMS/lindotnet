using System;
using System.Runtime.InteropServices;

namespace lindotnet.Classes.Wrapper.Implementation.Loader
{
	public class Structs
	{
		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		public struct VaListWindows
		{
			public IntPtr Pointer;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		public struct VaListLinuxX64
		{
			public UInt32 gp_offset;
			public UInt32 fp_offset;
			public IntPtr overflow_arg_area;
			public IntPtr reg_save_area;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		public struct MSVideoSize
		{
			public UInt32 width;
			public UInt32 height;
		}

		public struct BctbxList
		{
			public IntPtr next;
			public IntPtr prev;
			public IntPtr data;
		}
	}
}
