using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonConverterTest;

public class Sample : ISample
{
    public int Data1 { get; set; }
    public int Data2 { get; set; }
    public string Data3 { get; set; } = "";

    public Sample(int data1, int data2, string data3)
    {
        Data1 = data1;
        Data2 = data2;
        Data3 = data3;
    }
}
