using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLogTraceTest;

public class TestTraceListener : TraceListener
{
	private static readonly Logger _Logger = LogManager.GetCurrentClassLogger();

	public override void Write(string? message)
	{
		_Logger.Debug(message ?? "");
	}

	public override void WriteLine(string? message)
	{
		_Logger.Debug(message ?? "");
	}

	/// <summary>
	/// <see cref="TraceListener"/> クラスの新しいインスタンスを初期化します。
	/// </summary>
	public TestTraceListener()
	{
	}
}
