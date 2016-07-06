using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
	public class App
	{
		public string PathToAssembly { get; private set; }
		public string Code { get; set; }
		public List<string> Assemblies { get; set; }

		public App(string name, string code)
		{
			Assemblies = new List<string>();
			Assemblies.Add(code);
			Assemblies.Add(File.ReadAllText(@"C:\Users\beaulieue\Documents\Projects\Current\ConsoleApplication2\CSharpAICoder\SimpleCodeLv1.cs"));
			PathToAssembly = name + ".exe";

			Compile();
		}

		private void Compile()
		{
			CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

			CompilerParameters parameters = new CompilerParameters();
			parameters.GenerateExecutable = true;
			parameters.OutputAssembly = PathToAssembly;

			CompilerResults results = provider.CompileAssemblyFromSource(parameters, Assemblies.ToArray());

			if (results.Errors.Count == 0) PathToAssembly = results.PathToAssembly;
		}

		public string Run()
		{
			var p = new Process();
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.FileName = PathToAssembly;
			p.Start();
			p.WaitForExit();
			string output = p.StandardOutput.ReadToEnd();
			return output;
		}
	}
}
