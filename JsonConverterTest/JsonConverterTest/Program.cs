

using System.Text.Json;

namespace JsonConverterTest;

internal class Program
{
    static void Main(string[] args)
    {
        var program = new Program();
        
        program.Run();
    }

    public void Run()
    {
        Sample3();
    }

    public void Sample1()
    {
        ISample iSample = new Sample(1, 2, "test1");

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters =
            {

            }
        };

        // シリアライズは型でエラーにならない
        var jsonText = JsonSerializer.Serialize(iSample, options);

        // デシリアライズはインターフェースを解決できないため、エラーになる
        var dSample = JsonSerializer.Deserialize<ISample>(jsonText, options);
    }

    public void Sample2()
    {
        ISample iSample = new Sample(3, 4, "test2");

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters =
            {
                new InterfaceConverter<ISample, Sample>()
            }
        };

        var jsonText = JsonSerializer.Serialize(iSample, options);
        var dSample = JsonSerializer.Deserialize<ISample>(jsonText, options);
    }

    public void Sample3()
    {
        var sample = new SampleGenerics<int>(5, 6);

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters =
            {
            }
        };

        var jsonText = JsonSerializer.Serialize(sample, options);
        var dSample = JsonSerializer.Deserialize<SampleGenerics<double>>(jsonText, options);
    }
}