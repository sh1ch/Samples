using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThread_LockTest;

public class ReaderWriterLockSlimProcess : ProcessBase
{
    public ReaderWriterLockSlim _Locker = new ();
    public override string Name => nameof(ReaderWriterLockSlimProcess);

    public override void Increment(int counts)
    {
        for (int i = 0; i < counts; i++)
        {
            _Locker.EnterWriteLock();

            try
            {
                _Value += 1;
            }
            finally
            { 
                _Locker.ExitWriteLock(); 
            }
        }
    }
}
