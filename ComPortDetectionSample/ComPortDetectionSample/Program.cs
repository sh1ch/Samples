using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ComPortDetectionSample
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
            Console.WriteLine("------------------------------");
            Console.WriteLine("SERIALPORT 切り替え確認");
            Console.WriteLine("------------------------------");

            Console.WriteLine($"テストするクラスを選択します。{nameof(SerialPortWatcher)} = input 'w', {nameof(SerialPortObserver)} = input 'o'");
            var type = Console.ReadLine();

            if (type == "w")
            {
                TestSerialPortWatcher();
            }
            else if (type == "o")
            {
                TestSerialPortObserver();
            }
        }

        private void TestSerialPortObserver()
        {
            var observer = new SerialPortObserver();

            observer.Start();
            observer.IsIgnoredUnknown = true;

            observer.Changed += (sender, e) =>
            {
                Console.WriteLine($"変更通知：{e.Type}, NEW：{string.Join(",", e.NewPortNames)}, OLD:{string.Join(",", e.OldPortNames)}");
            };

            Console.WriteLine($"{nameof(SerialPortObserver)} のテストを開始します。");

            while (true)
            {
                // COMPORT の切り替え待機
            }
        }

        private void TestSerialPortWatcher()
        {
            var watcher = SerialPortWatcher.Instance;

            watcher.Start();

            watcher.Changed += (sender, e) =>
            {
                Console.WriteLine($"変更通知：{e.Type}, NEW：{string.Join(",", e.NewPortNames)}");
            };

            Console.WriteLine($"{nameof(SerialPortWatcher)} のテストを開始します。");

            while (true)
            {
                // COMPORT の切り替え待機
            }
        }

    }
}
