using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLog.Practice1.Sample;

[Target("sample2")]
public class SampleTargetWithLayout : TargetWithLayout
{
    [RequiredParameter]
    [DefaultParameter]
    public string Data { get; set; } = "";

    protected override void Write(LogEventInfo logEvent)
    {
        string logMessage = this.Layout.Render(logEvent);

        Console.WriteLine(logMessage);
    }
}
