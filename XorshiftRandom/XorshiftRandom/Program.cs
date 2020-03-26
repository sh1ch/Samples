using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace XorshiftRandom
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();

            Console.CursorVisible = false;

            program.Run();

            Console.CursorVisible = true;

            Console.ReadLine();
        }

        public void Run()
        {
            RangeTest();
            ImageTest();
        }

        public void ImageTest()
        {
            var random = new RandomState();

            random.SetSeed(DateTime.Now.Ticks);

            var width = 640;
            var height = 640;

            var bmp = new Bitmap(width, height);

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var r = random.Range(0, 1);
                    var color = r == 0 ? Color.White : Color.Black;

                    bmp.SetPixel(x, y, color);
                }
            }

            bmp.Save($"image-{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}.png", ImageFormat.Png);
        }

        public void RangeTest()
        {
            var random = new RandomState();
            random.SetSeed(DateTime.Now.Ticks);
            
            // 試行回数
            ulong count = 10000000L;

            // 範囲
            var min = 0;
            var max = 100;

            var maxW = 0.0D;
            var minW = 1.0D;

            for (ulong i = 0; i < count; i++)
            {
                var w = random.Range(min, max);

                if (maxW < w)
                {
                    maxW = w;
                }
                else if (minW > w)
                {
                    minW = w;
                }

                if (i % (ulong)(count / 10000) == 0)
                {
                    Console.Write("{0:00.0000} %", ((double)i / (double)count * 100));
                    Console.SetCursorPosition(0, Console.CursorTop);
                }
            }

            Console.WriteLine($"MaxValue = {maxW}");
            Console.WriteLine($"MinValue = {minW}");
        }


    }
}
