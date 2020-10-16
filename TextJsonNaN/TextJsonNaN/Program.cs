using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TextJsonNaN
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();

            program.Run();
        }

        public void Run()
        {
            // var json = @"{""NumberOne"":-1,""NumberTwo"":""NaN"",""NumberPositive"":""Infinity"",""NumberNegative"":""-Infinity""}";
            var json = WriteTest();

            ReadTest(json);
        }

        public string WriteTest()
        {
            var options = new JsonSerializerOptions
            {
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals,
            };

            var data = new ClassWithInts
            {
                NumberOne = -1,
                NumberTwo = double.NaN,
                NumberPositive = double.PositiveInfinity,
                NumberNegative = double.NegativeInfinity,
            };

            var json = JsonSerializer.Serialize(data, options);

            Console.WriteLine(json);

            return json;
        }

        public void ReadTest(string json)
        {
            var options = new JsonSerializerOptions
            {
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString | 
                                 System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals,
            };

            var data = JsonSerializer.Deserialize<ClassWithInts>(json, options);
        }
    }
}
