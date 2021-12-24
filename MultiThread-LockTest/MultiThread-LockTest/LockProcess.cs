using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThread_LockTest;

public class LockProcess : ProcessBase
{
    private object _Locker = new object();
    public override string Name => nameof(LockProcess);

    public override void Increment(int counts)
    {
        for (int i = 0; i < counts; i++)
        {
            lock (_Locker)
            {
                _Value += 1;
            }
        }
    }
}
