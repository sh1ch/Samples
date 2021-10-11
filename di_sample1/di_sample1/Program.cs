using System;
using Unity;
using Unity.Injection;

namespace di_sample1
{
    


    class Program
    {
        static void Main(string[] args)
        {
            var sample1 = new Sample1();
            var sample2 = new Sample2();

            sample1.Test();
            sample2.Test();

            Console.WriteLine("Hello World!");
        }
    }
}
