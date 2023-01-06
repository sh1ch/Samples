using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLog.Practice1;

public class Runner
{
    private readonly ILogger<Runner> _Logger;

    public Runner(ILogger<Runner> logger)
    {
        _Logger = logger;
    }

    public void DoAction(string name)
    {
        _Logger.LogDebug(20, "Doing hard work! {Action}", name);
    }
}
