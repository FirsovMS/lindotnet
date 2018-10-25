using lindotnet.Classes.Component.Implementation;
using System;
using System.Threading.Tasks;

namespace lindotnet.Classes.Helpers
{
	public static class ComponentExtensions
	{
        public static bool IsExist(this Call call)
        {
            return call != null;
        }

		public static async Task ExecuteWithDelay(Action action, int timeoutInMilliseconds)
		{
			await Task.Delay(timeoutInMilliseconds);
			action();
		}
	}
}
