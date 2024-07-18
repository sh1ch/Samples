using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLibrary;

public class Sample
{
	public void WriteLine(string text)
	{
		System.Diagnostics.Debug.WriteLine(text);
	}

	public static void WriteLineStatic(string text)
	{
		System.Diagnostics.Debug.WriteLine(text);
	}
}
