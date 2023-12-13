using System;

namespace TimeProviderTest; 

public class Program
{
	static void Main(string[] args)
	{
		var program = new Program();

		program.Run(args);
	}

	public void Run(string[] args)
	{
		var now = TimeProvider.System.GetLocalNow();
		var utcNow = TimeProvider.System.GetUtcNow();

		Console.WriteLine(now);
		Console.WriteLine(utcNow);

		var now2 = DateTime.Now;
		var utcNow2 = DateTimeOffset.UtcNow;
		
		Console.WriteLine(now2);
		Console.WriteLine(utcNow2);
	}
}

public class TimeService
{
	private readonly TimeProvider _TimeProvider;

	public TimeService(TimeProvider timeProvider) => _TimeProvider = timeProvider;

	public bool IsNoon()
	{
		var now = _TimeProvider.GetLocalNow();
		
		return now.Hour == 12;
	}
}