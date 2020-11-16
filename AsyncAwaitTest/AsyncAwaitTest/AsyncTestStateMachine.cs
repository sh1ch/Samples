using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwaitTest
{
    public struct AsyncTestStateMachine : IAsyncStateMachine
    {
        private TaskAwaiter _TaskAwaiter;

        public int State { get; set; }
        public AsyncVoidMethodBuilder Builder { get; set; }

        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {

        }

        public void MoveNext()
        {
            int num = State;

            try
            {
                TaskAwaiter awaiter;
                
                if (num != 0)
                {
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

                    awaiter = Task.Delay(5000).GetAwaiter();

                    if (!awaiter.IsCompleted)
                    {
                        num = State = 0;

                        _TaskAwaiter = awaiter;
                        AsyncTestStateMachine stateMachine = this;

                        Builder.AwaitUnsafeOnCompleted<TaskAwaiter, AsyncTestStateMachine>(ref awaiter, ref stateMachine);

                        return;
                    }
                }
                else
                {
                    awaiter = _TaskAwaiter;

                    _TaskAwaiter = default(TaskAwaiter);

                    num = State = -1;
                }

                awaiter.GetResult();

                Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            }
            catch (Exception ex)
            {
                State = -2;
                Builder.SetException(ex);

                return;
            }

            State = -2;
            Builder.SetResult();
        }

    }
}