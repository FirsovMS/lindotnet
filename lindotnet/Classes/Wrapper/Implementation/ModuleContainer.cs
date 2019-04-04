using lindotnet.Classes.Helpers;
using lindotnet.Classes.Wrapper.Implementation.Loader;
using lindotnet.Classes.Wrapper.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace lindotnet.Classes.Wrapper.Implementation
{
	public static class ModuleContainer
	{
		private static readonly ConcurrentDictionary<string, object> _modules = new ConcurrentDictionary<string, object>();

		public static TValue GetModule<TValue>()
			where TValue : class
		{
			object result = default(TValue);
			_modules.TryGetValue(typeof(TValue).Name, out result);
			return (TValue)result;
		}

		public static void SetModule<TValue>(object module)
			where TValue : class
		{
			var type = typeof(TValue);
			CheckModuleBindings(type);
			_modules.AddOrUpdate(type.Name, module, (key, value) => module);
		}

		private static void CheckModuleBindings(Type module)
		{
			var linphoneDLL = DllLoader.LoadLibrary(Constants.LIBNAME);
			foreach (MethodInfo methodInfo in module.GetMethods(BindingFlags.Public | BindingFlags.Static))
			{
				var methodName = methodInfo.Name;
				if (DllLoader.ProcedureCall(linphoneDLL, methodName).IsZero())
				{
					throw new EntryPointNotFoundException($"Procedure Mapping isn't right -> {methodName} not found.");
				}
			}
			DllLoader.FreeLibrary(linphoneDLL);
		}
	}
}
