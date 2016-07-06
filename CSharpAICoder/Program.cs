using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpAICoder
{
	class Program
	{
		static void Main(string[] args)
		{
			new Tests().CodeHello(typeof(SimpleCodeLv1));

//			var app = new App("test", @"
//using System;
//using CSharpAICoder;

//namespace CoderAlgorithm
//{
//	public class Program
//	{
//		public static void Main(string[] args)
//		{
//			var simpleCode = new SimpleCodeLv1();
//			simpleCode.Prompt();
//			simpleCode.ChangeOutput();
//			simpleCode.Write();
//		}
//	}
//}");

//			app.Run();
		}
	}
}
