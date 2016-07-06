using System;
using CSharpAICoder;

namespace CoderAlgorithm
{
	public class Program
	{
		public void Main(string[] args)
		{
			var simpleCode = new SimpleCodeLv1();
			simpleCode.Prompt();
			simpleCode.Prompt();
			simpleCode.Write();

		}
	}
}