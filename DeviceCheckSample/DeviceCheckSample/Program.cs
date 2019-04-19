using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DeviceCheckSample
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
            var devices = AudioDevice.Find();

            if (devices.Count() > 0)
            {
                foreach (var dev in devices)
                {
                    Console.WriteLine($"{dev.Id}:{dev.Name}");
                }
            }
            else
            {
                Console.WriteLine("オーディオデバイスが存在しません。");
            }
        }

        
    }
}
