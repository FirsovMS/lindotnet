using System;
using System.Runtime.InteropServices;

namespace lindotnet.Classes.Helpers
{
	public static class MarshalingExtensions
	{
		public static bool IsZero(this IntPtr ptr) => ptr == IntPtr.Zero;

		public static bool IsNonZero(this IntPtr ptr) => !IsZero(ptr);

		/// <summary>
		/// Free unmanaged memory
		/// </summary>
		/// <param name="ptr"></param>
		public static void Free(this IntPtr ptr)
		{
			if (ptr.IsNonZero())
			{
				Marshal.FreeHGlobal(ptr);
			}
		}

		public static IntPtr ToIntPtr<T>(this T obj, bool deleteOld = false)
			where T : struct
		{
			var result = Marshal.AllocHGlobal(Marshal.SizeOf(obj));
			Marshal.StructureToPtr(obj, result, false);
			return result;
		}
	}
}
