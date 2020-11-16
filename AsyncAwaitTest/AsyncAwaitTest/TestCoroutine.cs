using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwaitTest
{
    public class TestCoroutine : IEnumerator<object>, IDisposable, IEnumerator
    {
        private int _State;
        private object _Current;

        public TestCoroutine _This;

        object IEnumerator<object>.Current => _Current;
        public object Current => _Current;

        public TestCoroutine(int state) => _State = state;

        public void Dispose() { }

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public bool MoveNext()
        {
            switch (_State)
            {
                default:
                    return false;
                case 0:
                    _State = -1;
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                    _Current = null;
                    _State = 1;
                    return true;
                case 1:
                    _State = -1;
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                    _Current = null;
                    _State = 2;
                    return true;
                case 2:
                    _State = -1;
                    return false;
            }
        }

        public IEnumerator Coroutine()
        {
            var coroutine = new TestCoroutine(0);

            coroutine._This = this;

            return coroutine;
        }
    }
}
