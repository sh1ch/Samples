using System;

namespace DisposableTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var program = new Program();

            program.Run();
        }

        public DisposableEventHandler<int> Sample = new DisposableEventHandler<int>();

        public void Run()
        {
            Console.WriteLine("---");

            Test1();
            Sample.Dispose();

            Console.WriteLine("---");

            Test2();
        }

        public void Test1()
        {
            Sample.SubScribe((sender, args) =>
            {
                Console.WriteLine($"{sender}.{args}");
            });

            Sample.Raise(this, 1);
            Sample.Raise(this, 2);
        }

        public void Test2()
        {
            using (var sample = new DisposableEventHandler<string>())
            {
                sample.SubScribe((sender, args) =>
                {
                    Console.WriteLine($"{sender}={args}");
                });

                sample.SubScribe((sender, args) =>
                {
                    Console.WriteLine($"{sender}+{args}");
                });

                sample.Raise(this, "ランス");
                sample.Raise(this, "シィル");
            }
        }
    }
}
