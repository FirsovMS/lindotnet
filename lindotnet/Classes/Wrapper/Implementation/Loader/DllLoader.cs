using lindotnet.Classes.Helpers;
using LoggingAPI;
using System;
using System.Runtime.InteropServices;
using static lindotnet.Classes.Wrapper.Implementation.Loader.Structs;

namespace lindotnet.Classes.Wrapper.Implementation.Loader
{
	/// <summary>
	/// Cross-platform native loading utils.
	/// By http://dimitry-i.blogspot.ru/2013/01/mononet-how-to-dynamically-load-native.html
	/// </summary>
	internal static class DllLoader
	{
		private static bool isLinux = false;

		static DllLoader()
		{
			int platformId = (int)Environment.OSVersion.Platform;
			isLinux = (platformId == 4) || (platformId == 6) || (platformId == 128);
		}

		public static IntPtr DoLoadLibrary(string fileName)
		{
			if (isLinux)
			{
				return LoadLinuxDlls.dlopen(fileName, Constants.RTLD_NOW);
			}
			else
			{
				return LoadWindowsDlls.LoadLibrary(fileName);
			}
		}

		public static void DoFreeLibrary(IntPtr handle)
		{
			if (isLinux)
			{
				LoadLinuxDlls.dlclose(handle);
			}
			else
			{
				LoadWindowsDlls.FreeLibrary(handle);
			}
		}

		public static IntPtr DoGetProcAddress(IntPtr dllHandle, string name)
		{
			if (isLinux)
			{
				return DoGetProcAddressOnLinux(dllHandle, name);
			}
			else
			{
				return LoadWindowsDlls.GetProcAddress(dllHandle, name);
			}
		}

		public static string ProcessVAlist(string format, IntPtr args)
		{
			if (isLinux)
			{
				return ProcessVAListOnLinux(format, args);
			}
			else
			{
				return ProcessVAListOnWindows(format, args);
			}
		}

		private static string ProcessVAListOnWindows(string format, IntPtr args)
		{
			var result = string.Empty;
			int byteLength = LoadWindowsDlls.vscprintf(format, args) + 1;
			IntPtr buffer = Marshal.AllocHGlobal(byteLength);

			try
			{
				LoadWindowsDlls.vsprintf(buffer, format, args);

				return Marshal.PtrToStringAnsi(buffer);
			}
			catch (Exception ex)
			{
				Logger.Error("Processing virtual addresses failed!", ex, Level.Critical);
			}
			finally
			{
				Marshal.FreeHGlobal(buffer);
			}
			return result;
		}

		private static string ProcessVAListOnLinux(string format, IntPtr args)
		{
			var result = string.Empty;
			if (Environment.Is64BitOperatingSystem)
			{
				var listStructure = Marshal.PtrToStructure(args, typeof(VaListLinuxX64));
				int byteLength = 0;
				IntPtr listPointer = Marshal.AllocHGlobal(Marshal.SizeOf(listStructure));

				try
				{
					Marshal.StructureToPtr(listStructure, listPointer, false);
					byteLength = LoadLinuxDlls.vsnprintf(IntPtr.Zero, UIntPtr.Zero, format, listPointer) + 1;
				}
				catch (Exception ex)
				{
					Logger.Error("Can't get bytes length of structure!", ex, Level.Critical);
				}
				finally
				{
					Marshal.FreeHGlobal(listPointer);
				}

				IntPtr buffer = Marshal.AllocHGlobal(byteLength);
				try
				{
					listPointer = Marshal.AllocHGlobal(Marshal.SizeOf(listStructure));
					try
					{
						Marshal.StructureToPtr(listStructure, listPointer, false);
						LoadLinuxDlls.vsprintf(buffer, format, listPointer);
						return Marshal.PtrToStringAnsi(buffer);
					}
					catch (Exception ex)
					{
						Logger.Error("failed processing structure to Ptr!", ex, Level.Critical);
					}
					finally
					{
						Marshal.FreeHGlobal(listPointer);
					}
				}
				catch (Exception ex)
				{
					Logger.Error("can't allocate memory for structure!", ex, Level.Fatal);
				}
				finally
				{
					Marshal.FreeHGlobal(buffer);
				}
			}
			else
			{
				int byteLength = LoadLinuxDlls.vsnprintf(IntPtr.Zero, UIntPtr.Zero, format, args) + 1;
				IntPtr buffer = Marshal.AllocHGlobal(byteLength);
				try
				{
					LoadLinuxDlls.vsprintf(buffer, format, args);
					return Marshal.PtrToStringAnsi(buffer);
				}
				catch (Exception ex)
				{
					Logger.Error("can't process ptr to string value!", ex, Level.Critical);
				}
				finally
				{
					Marshal.FreeHGlobal(buffer);
				}
			}
			return result;
		}

		private static IntPtr DoGetProcAddressOnLinux(IntPtr dllHandle, string name)
		{
			// clear previous errors if any
			LoadLinuxDlls.dlerror();
			var res = LoadLinuxDlls.dlsym(dllHandle, name);
			var errPtr = LoadLinuxDlls.dlerror();
			if (errPtr.IsNonZero())
			{
				throw new Exception("dlsym: " + Marshal.PtrToStringAnsi(errPtr));
			}
			return res;
		}
	}
}
