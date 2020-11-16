using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwaitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();

            program.Run();
        }

        private void Run()
        {
            CoroutineTest();

            Console.ReadLine();
        }

        private void AsyncTest()
        {
            var stateMachine = new AsyncTestStateMachine();

            stateMachine.Builder = AsyncVoidMethodBuilder.Create();
            stateMachine.State = -1;

            stateMachine.Builder.Start(ref stateMachine);
        }

        private async void AsyncTest2()
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

            await Task.Delay(5000);

            // 別々のスレッドで実行される
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
        }

        private void CoroutineTest()
        {
            var coroutine = new TestCoroutine(0);

            while (coroutine.MoveNext())
            {
                ;
            }
        }
    }
}
