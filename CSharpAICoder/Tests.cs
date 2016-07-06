using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSharpAICoder
{
	class Tests
	{
		public void RandomString(int lenght)
		{
			//var ga = new GeneticAlgorithm<char>(new char[] { '0', '1' });

			List<char> chars = new List<char>();
			for (int i = 0; i <= 255; i++) chars.Add((char)i);

			var ga = new GeneticAlgorithm<char>(chars, lenght, 10000);
			ga.PopulationCount = 1000;

			var result = ga.Run(0.6, 0.002);
			Trace.WriteLine(new string(ga.Goal.ToArray()));
			Trace.WriteLine(new string(result.ToArray()));
		}

		public void SpecificString(string s)
		{
			//var ga = new GeneticAlgorithm<char>(new char[] { '0', '1' });

			List<char> chars = new List<char>();
			for (int i = 0; i <= 255; i++) chars.Add((char)i);

			var ga = new GeneticAlgorithm<char>(chars, new List<char>(s), 10000);
			ga.PopulationCount = 1000;

			var result = ga.Run(0.6, 0.002);
			Trace.WriteLine(new string(ga.Goal.ToArray()));
			Trace.WriteLine(new string(result.ToArray()));
		}

		public void CodeHello(Type type)
		{
			var possibleCommands = type.GetMethods(BindingFlags.Instance|BindingFlags.Public|BindingFlags.InvokeMethod|BindingFlags.DeclaredOnly)
				.Where(m => !m.IsSpecialName);

			var coder = new CoderAlgorithm(possibleCommands, "Prompt: Eric\r\nHello Eric\r\n");
			var result = coder.Run(0.6, 0.002);
			Trace.WriteLine(coder.Goal);
			Trace.WriteLine(result.Code);
		}
	}
}
