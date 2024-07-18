using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLogTraceTest;

public class LevelTraceListener : TraceListener
{
	private static readonly Logger _Logger = LogManager.GetCurrentClassLogger();
	private Action<string> _Action;

	public LogLevel Level { get; }

	public override void Write(string? message)
	{
		_Action.Invoke(message ?? "");
	}

	public override void WriteLine(string? message)
	{
		_Action.Invoke(message ?? "");
	}

	/// <summary>
	/// <see cref="LevelTraceListener"/> クラスの新しいインスタンスを初期化します。
	/// </summary>
	/// <param name="level">ログの出力レベル。</param>
	public LevelTraceListener(NLog.LogLevel level)
	{
		Level = level;

		Action<string> action = level.Name switch
		{
			"Trace" => Trace,
			"Debug" => Debug,
			"Info" => Info,
			"Warn" => Warn,
			"Error" => Error,
			"Fatal" => Fatal,
			"Off" => Off,
			_ => Debug,
		};

		_Action = action;
	}

	private void Trace(string message) => _Logger.Trace(message);
	private void Debug(string message) => _Logger.Debug(message);
	private void Info(string message) => _Logger.Info(message);
	private void Warn(string message) => _Logger.Warn(message);
	private void Error(string message) => _Logger.Error(message);
	private void Fatal(string message) => _Logger.Fatal(message);
	private void Off(string message)
	{
		// NOP
	}
}
