using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParserSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var text1 = "10.2 + 20.2 + 5.2";
            var text2 = "10 * (20.5 + 5.5)";
            var text3 = "1 + 2 * ( 3 + 4 * ( 10 / 10 ) ) + 5 * ( 6 * ( 7 + 8 ) + ( 9 + 10 ) )";
            var text4 = "1";
            var text5 = "[H%]*([Y]/([X]+[Y]*[Z]))*(2.0141/1.0079)";

            var p = new Parser();
            var n1 = p.Parse(text1);
            var n2 = p.Parse(text2);
            var n3 = p.Parse(text3);
            var n4 = p.Parse(text4);
            var n5 = p.Parse(text5);

            Console.WriteLine("------");
            n1.Print();
            Console.WriteLine("------");
            n2.Print();
            Console.WriteLine("------");
            n3.Print();
            Console.WriteLine("------");
            n4.Print();
            Console.WriteLine("------");
            n5.Print();
            Console.WriteLine("------");

            Console.WriteLine($"ans:{p.Eval(n1)}");
            Console.WriteLine($"ans:{p.Eval(n2)}");
            Console.WriteLine($"ans:{p.Eval(n3)}");
            Console.WriteLine($"ans:{p.Eval(n4)}");

        }
    }
}
