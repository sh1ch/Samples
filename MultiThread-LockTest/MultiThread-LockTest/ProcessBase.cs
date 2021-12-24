using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThread_LockTest;

public abstract class ProcessBase : IProcess
{
    protected int _Value = 0;

    private List<(int, long)> _ExecutedInfos = new ();

    public abstract string Name { get; }
    public abstract void Increment(int counts);

    public void Run(int threadCount, int testValue, bool needLog)
    {
        if (needLog)
        {
            Console.WriteLine($"Start {Name} - thread no:{Thread.CurrentThread.ManagedThreadId:D3}");
        }

        var tasks = new List<Task>();
        var loops = testValue / threadCount; // test 値を指定数のスレッドで割って、加算プロセスを実行する
        var stopWatch = new Stopwatch();

        _Value = 0;

        stopWatch.Start();

        for (int i = 0; i < threadCount; ++i)
        {
            var task = Task.Run(() => 
            {
                Increment(loops);
            });

            tasks.Add(task);
        }

        Task.WhenAll(tasks).Wait();
        stopWatch.Stop();

        // 結果を保存
        _ExecutedInfos.Add((_Value, stopWatch.ElapsedMilliseconds));

        if (needLog)
        {
            Console.WriteLine($"End {Name} - thread no:{Thread.CurrentThread.ManagedThreadId:00} value: {_Value:#,0} time: {stopWatch.ElapsedMilliseconds:#,0} ms");
        }
    }

    public void WriteAverageLog()
    {
        var averagedValue = _ExecutedInfos.Average(p => p.Item1);
        var maxValue = _ExecutedInfos.Max(p => p.Item1);
        var minValue = _ExecutedInfos.Min(p => p.Item1);
        var averagedTime = _ExecutedInfos.Average(p => p.Item2);
        var maxTime = _ExecutedInfos.Max(p => p.Item2);
        var minTime = _ExecutedInfos.Min(p => p.Item2);

        Console.WriteLine($"Average {Name} - value: {averagedValue:#,0}(Max:{maxValue:#,0} Min:{minValue:#,0}), time: {averagedTime:#,0}(Max:{maxTime:#,0} Min:{minTime:#,0})");
    }
}
