using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThread_LockTest;

public class InterlockedProcess : ProcessBase
{
    public override string Name => nameof(InterlockedProcess);

    public override void Increment(int counts)
    {
        for (int i = 0; i < counts; i++)
        {
            Interlocked.Increment(ref _Value);
        }
    }
}
