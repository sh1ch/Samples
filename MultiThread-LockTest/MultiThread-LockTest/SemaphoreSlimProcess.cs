using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThread_LockTest;

public class SemaphoreSlimProcess : ProcessBase
{
    private SemaphoreSlim _Locker = new (1, 1);
    public override string Name => nameof(SemaphoreSlimProcess);

    public override void Increment(int counts)
    {
        for (int i = 0; i < counts; i++)
        {
            _Locker.Wait();

            try
            {
                _Value += 1;
            }
            finally
            {
                _Locker.Release();
            }
        }
    }
}
