using System;
using System.Runtime.InteropServices.JavaScript;
using TimeProviderTest;

namespace TestProject;

public class Tests
{
	public class NoonTimeProvider : TimeProvider
	{
		public override DateTimeOffset GetUtcNow()
		{
			return new DateTimeOffset(2023, 12, 1, 3, 0, 0, TimeSpan.Zero);
		}

		public override TimeZoneInfo LocalTimeZone => 
			TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
	}

	[SetUp]
	public void Setup()
	{
	}

	[Test]
	public void TestTimeProvider()
	{
		var testTimeProvider = new NoonTimeProvider();

		var testService = new TimeService(testTimeProvider);
		var isNoon = testService.IsNoon();

		Assert.IsTrue(isNoon);
	}

	[Test]
	public void TestITimer()
	{
		var testTimeProvider = new FakeTimeProvider();
		var result = false;

		var itimer = testTimeProvider.CreateTimer(
			_ =>
			{
				// 時間経過後にすること
				result = true;
			}, 
			state:null, 
			dueTime: TimeSpan.FromSeconds(1000),
			period: Timeout.InfiniteTimeSpan);

		testTimeProvider.Advance(TimeSpan.FromSeconds(1000));

		Assert.True(result);

		ITimer
	}
}