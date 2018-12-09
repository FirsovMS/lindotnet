using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoClient
{
	class Program
	{
		private static string EXIT_SYMBOL = "q";

		private static IEnumerable<string> help = new List<string>()
		{
			"press 'q' to exit",
		};

		private static Application app;

		static void Main(string[] args)
		{
			Help();

			Task.Factory.StartNew(() =>
			{
				app = new Application();
			});

			var input = string.Empty;
			while (!string.Equals(input, EXIT_SYMBOL, StringComparison.InvariantCultureIgnoreCase))
			{
				input = Console.ReadLine();
			}
		}

		static void Help()
		{
			Console.WriteLine(string.Join(Environment.NewLine, help));
		}
	}
}
