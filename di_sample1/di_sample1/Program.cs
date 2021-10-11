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
            var sample3 = new Sample3();
            var sample4 = new Sample4();

            sample1.Test();
            Console.WriteLine("--");
            sample2.Test();
            Console.WriteLine("--");
            sample3.Test();
            Console.WriteLine("--");
            sample4.Test();
        }
    }
}
