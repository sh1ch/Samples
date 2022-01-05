using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RetryTest;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");

        var program = new Program();

        program.Run();
    }

    public void Run()
    {
        try
        {
            var task = Retry.InvokeAsync(TestAction, 5, 1000);
            task.Wait();

            Console.WriteLine("Wait ended.");
        }
        catch
        {
            Console.WriteLine("task has error.");
        }

        Console.ReadLine();
    }

    public void TestAction()
    {
        Console.WriteLine("Test action called.");

        throw new Exception();
    }
}

public class Retry
{
    private static readonly object _Locker = new ();

    /// <summary>
    /// 実行回数を取得または設定します。
    /// </summary>
    public static int Attempts { get; set; } = 4;

    /// <summary>
    /// 実行に失敗したときに発生させるディレイ (ms) を取得または設定します。
    /// </summary>
    public static int SleepMilliseconds { get; set; } = 1000;

    public static async Task<T> InvokeAsync<T>(Func<T> func) => await InvokeAsync(func, Attempts, SleepMilliseconds);

    /// <summary>
    /// 指定した <see cref="Func{TResult}"/> を実行します。処理に失敗した場合、試行回数まで繰り返し実行します。
    /// </summary>
    /// <typeparam name="T">実行メソッドの戻り値。</typeparam>
    /// <param name="func">実行メソッド。</param>
    /// <param name="attempts">試行回数。</param>
    /// <param name="sleepMilliseconds">試行の間隔。</param>
    /// <returns>実行タスクのハンドル。</returns>
    public static async Task<T> InvokeAsync<T>(Func<T> func, int attempts, int sleepMilliseconds,
                                [CallerMemberName] string memberName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
    {
        while (true)
        {
            try
            {
                T? result;

                lock (_Locker)
                {
                    result = func();
                }

                return result;
            }
            catch (Exception ex)
            {
                if (--attempts <= 0)
                {
                    Debug.WriteLine($"{ex.GetType()} caught: could not invoke. caller: {memberName} - {sourceFilePath} ({sourceLineNumber})");
                    throw;
                }

                Debug.WriteLine($"{ex.GetType()} caught: retry after {sleepMilliseconds} ms (left try: {attempts})");
                await Task.Delay(sleepMilliseconds);
            }
        }
    }

    public static async Task InvokeAsync(Action action) => await InvokeAsync(action, Attempts, SleepMilliseconds);

    public static async Task InvokeAsync(Action action, int attempts, int sleepMilliseconds,
                             [CallerMemberName] string memberName = "", 
                             [CallerFilePath] string sourceFilePath = "", 
                             [CallerLineNumber] int sourceLineNumber = 0)
    {
        while (true)
        {
            try
            {
                lock (_Locker)
                {
                     action();
                }

                break;
            }
            catch (Exception ex)
            {
                if (--attempts <= 0)
                {
                    Debug.WriteLine($"{ex.GetType()} caught: could not invoke. caller: {memberName} - {sourceFilePath} ({sourceLineNumber})");
                    throw;
                }

                Debug.WriteLine($"{ex.GetType()} caught: retry after {sleepMilliseconds} ms (left try: {attempts})");
                await Task.Delay(sleepMilliseconds);
            }
        }
    }

}
