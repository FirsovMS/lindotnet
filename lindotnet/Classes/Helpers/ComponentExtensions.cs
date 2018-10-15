using lindotnet.Classes.Component.Implementation;

namespace lindotnet.Classes.Helpers
{
	public static class ComponentExtensions
	{
        public static bool IsExist(this Call call)
        {
            return call != null;
        }
    }
}
