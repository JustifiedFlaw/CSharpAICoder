using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace CSharpAICoder
{
	public class CommandWrap
	{
		public MethodInfo MethodInfo { get; set; }
		public object[] Parameters { get; set; }

		public CommandWrap()
		{
			Parameters = new List<object>();
		}

		public CommandWrap(params object[] parameters)
		{
			Parameters = parameters;
		}
	}
}
