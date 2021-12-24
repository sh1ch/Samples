using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThread_LockTest;

public class MutexProcess : ProcessBase
{
    private Mutex _Locker = new ();
    public override string Name => nameof(MutexProcess);

    public override void Increment(int counts)
    {
        for (int i = 0; i < counts; i++)
        {
            _Locker.WaitOne();

            try
            {
                _Value += 1;
            }
            finally
            {
                _Locker.ReleaseMutex();
            }
        }
    }
}
