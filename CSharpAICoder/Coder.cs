using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
	public static class Coder
	{
		public static string WriteCode(List<MethodInfo> methods)
		{
			string code = "";
			foreach (var m in methods)
			{
				code += "simpleCode." + m.Name + "();\r\n";
			}

			code = @"using System;
using ConsoleApplication2;

namespace CoderAlgorithm
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var simpleCode = new SimpleCodeLv1();
		" + code + @"
		}
	}
}";

			return code;
		}
	}
}
