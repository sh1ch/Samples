using System;
using System.Collections.Generic;
using System.Linq;

namespace MultiThread_LockTest;

public class Program
{
    const int _ThreadCount = 4;
    const int _TestValue = 1000000; // _ThreadCount で割れる数でテストすること
    const int _TestCount = 100;      // 実行回数（実行時間と AverageLog に影響）

    public static void Main(string[] args)
    {
        Console.WriteLine($"Run test {_TestCount:#,0} times. Info(thread count:{_ThreadCount:#,0} value:{_TestValue:#,0})");

        ThreadPool.SetMinThreads(_ThreadCount, _ThreadCount);
        ThreadPool.SetMaxThreads(_ThreadCount, _ThreadCount);

        var processes = new List<IProcess>
        {
            new NoLockedProcess(),
            new LockProcess(),
            new InterlockedProcess(),
            new ReaderWriterLockSlimProcess(),
            new SemaphoreSlimProcess(),
            new MutexProcess(),
        };

        foreach (var process in processes)
        {
            for (int i = 0; i < _TestCount; i++)
            {
                process.Run(_ThreadCount, _TestValue, false);
            }
        }

        Console.WriteLine("--"); // 改行

        foreach (var process in processes)
        {
            process.WriteAverageLog();
        }
    }
}
