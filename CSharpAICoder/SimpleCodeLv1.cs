using System;

namespace ConsoleApplication2
{
	public class SimpleCodeLv1
	{
		public string Input { get; private set; }
		public string Output { get; set; }
		public void Prompt() { Console.Write("Prompt: "); Input = "Eric"; Console.WriteLine(Input); }
		public void Write() { Console.WriteLine(Output); }
		public void ChangeOutput() { Output = "Hello " + Input; }
	}
}
