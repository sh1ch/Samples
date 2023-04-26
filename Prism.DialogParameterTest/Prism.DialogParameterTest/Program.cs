using Prism.DialogParameterTest.Samples;
using System;

namespace Prism.DialogParameterTest;

internal class Program
{
    static void Main(string[] args)
    {
        var program = new Program();
        
        program.Run();

        Console.ReadKey();
    }

    private void Run()
    {
        var parameter = new Parameter();

        SetTest(parameter);
        GetTest(parameter);
    }

    private void SetTest(Parameter parameter)
    {
        parameter.Add("key1", "test data");
        parameter.Add("key2", 1);
        parameter.Add("key3", Enum1.Sample3);
        parameter.Add("key4", new Class1 { Data1 = 111, Data2 = "test 222" });
    }

    private void GetTest(Parameter parameter)
    {
        Console.WriteLine($"count = {parameter.Count}");
        Console.WriteLine(parameter["key1"]);
        Console.WriteLine(parameter["key2"]);
        Console.WriteLine(parameter["key3"]);
        Console.WriteLine(parameter["key4"]);

        var isSuccessed1 = parameter.TryGetValue<Enum1>("key3", out var e);

        if (isSuccessed1)
        {
            Console.WriteLine(e);
        }

        var isSuccessed2 = parameter.TryGetValue<Class1>("key4", out var c);

        if (isSuccessed2 && c != null)
        {
            Console.WriteLine(c.Data1);
            Console.WriteLine(c.Data2);
        }
    }
}