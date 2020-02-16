using System;

namespace EncryptHashSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();

            program.Run();
        }

        private void Run()
        {
            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(HashGenerator.Generate("password"));
            }
        }
    }
}
