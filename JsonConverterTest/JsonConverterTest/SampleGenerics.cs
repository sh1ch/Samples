using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonConverterTest;

internal class SampleGenerics<T> where T : struct
{
    public T Value1 { get; set; }
    public T Value2 { get; set; }

    public SampleGenerics(T value1, T value2)
    {
        Value1 = value1;
        Value2 = value2;
    }
}
