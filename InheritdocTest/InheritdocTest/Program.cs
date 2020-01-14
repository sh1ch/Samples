using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InheritdocTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var sample = new Sample();

            sample.Occurred += (s, a) => 
            {
                var data = s as Sample;
                Console.WriteLine($"{data?.Property1 ?? "Unk"} : Event Occurred.");
            };

            sample.Property1 = "Main";
            sample.Method1();
        }
    }
}
