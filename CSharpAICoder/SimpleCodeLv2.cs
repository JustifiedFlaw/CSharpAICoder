using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
	public class SimpleCodeLv2 //Like lv1 but with parameters on methods
	{
		public string Input { get; private set; }
		public string Output { get; set; }
		public void Prompt() { Input = Console.ReadLine(); }
		public void Write() { Console.WriteLine(Output); }
		public void ChangeOutput(string prefix) { Output = prefix + Input; }
	}
}
