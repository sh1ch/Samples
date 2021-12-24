using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThread_LockTest;

/// <summary>
/// <see cref="NoLockedProcess"/> クラスは、ロック制御をしていない実装パターンです。
/// </summary>
public class NoLockedProcess : ProcessBase
{
    public override string Name => nameof(NoLockedProcess);

    public override void Increment(int counts)
    {
        for (int i = 0; i < counts; i++)
        {
            _Value += 1;
        }
    }
}
