using NLog;
using System;
using System.Diagnostics;
using TestLibrary;

namespace NLogTraceTest;

internal class Program
{
	static void Main(string[] args)
	{
		var program = new Program();

		program.Run(args);
	}

	public void Run(string[] args)
	{
		Trace.Listeners.Clear(); // デフォルトの出力をクリア
		Trace.Listeners.Add(new LevelTraceListener(LogLevel.Info));

		Debug.WriteLine("test 1.");

		var sample = new Sample();

		sample.WriteLine("test 2.");

		Sample.WriteLineStatic("test 3.");
	}
}