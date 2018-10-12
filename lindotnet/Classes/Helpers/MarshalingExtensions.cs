using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace lindotnet.Classes.Helpers
{
	public static class MarshalingExtensions
	{
		public static bool IsZero(this IntPtr ptr)
		{
			return ptr == IntPtr.Zero;
		}

		public static bool IsNonZero(this IntPtr ptr)
		{
			return !IsZero(ptr);
		}

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
			where T : new()
		{
			var result = Marshal.AllocHGlobal(Marshal.SizeOf(obj));
			Marshal.StructureToPtr(obj, result, false);
			return result;
		}

		public static IEnumerable<string> ToStringCollection(this IntPtr collectionPtr)
		{
			var result = new List<string>();

			if (collectionPtr.IsNonZero())
			{
				var element = Marshal.ReadIntPtr(collectionPtr);
				string temp = null;
				while (element.IsNonZero())
				{
					if (TryConvert(element, out temp))
					{
						result.Add(temp);
					}
					collectionPtr = new IntPtr(collectionPtr.ToInt64() + IntPtr.Size);
					element = Marshal.ReadIntPtr(collectionPtr);
				}
			}

			return result;
		}

		public static bool TryConvert(IntPtr ptr, out string convertedString)
		{
			convertedString = string.Empty;
			if (ptr.IsNonZero())
			{
				convertedString = Marshal.PtrToStringAnsi(ptr);
				return !string.IsNullOrWhiteSpace(convertedString);
			}
			return false;
		}
	}
}
