using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThread_LockTest;

public interface IProcess
{
    public void Run(int threadCount, int testValue, bool needLog);
    public void WriteAverageLog();
}
