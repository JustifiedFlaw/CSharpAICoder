using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpAICoder
{
	public class SimpleCodeLv2 //Like lv1 but with parameters on methods
	{
		public string Input { get; private set; }
		public string Output { get; set; }
		public void Prompt() { Console.Write("Prompt: "); Input = "Eric"; Console.WriteLine(Input); }
		public void Write() { Console.WriteLine(Output); }
		public void ChangeOutput(string prefix) { Output = prefix + Input; }
	}
}
