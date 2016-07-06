using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSharpAICoder
{
	public class CodeWrap
	{
		public string Code { get; internal set; } = "";
		public List<MethodInfo> Methods { get; internal set; }
		public double Fitness { get; internal set; }
		public string Output { get; internal set; } = "";

		public string WriteCode()
		{
			Code = "";
			foreach (var m in Methods)
			{
				Code += "simpleCode." + m.Name + "();\r\n";
			}

			Code = @"using System;
using CSharpAICoder;

namespace CoderAlgorithm
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var simpleCode = new SimpleCodeLv1();
		" + Code + @"
		}
	}
}";

			return Code;
		}
	}
}
