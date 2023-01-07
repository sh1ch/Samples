using NLog.Config;
using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLog.Practice1.Sample;

[LayoutRenderer("sample")]
public class SampleLayoutRenderer : LayoutRenderer
{
    [RequiredParameter]
    public string? Option1 { get; set; } = null;

    public string? Option2 { get; set; } = null;

    [DefaultParameter]
    public string Default { get; set; } = "";

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
        builder.Append($"sample={Default} and Op1={Option1 ?? "null"} OP2={Option2 ?? "null"} ");
    }
}
