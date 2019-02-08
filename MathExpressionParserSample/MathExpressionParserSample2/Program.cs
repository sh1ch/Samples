using MathExpressionParserSample2.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParserSample2
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new TextToNode();
            var lexicalAnalyzer = new LexicalAnalysis();

            while (true)
            {
                var expression = new Expression();

                Console.WriteLine("------------");
                Console.WriteLine($"テストする式を入力してください（式の例は sample と入力ください）");

                var text = Console.ReadLine();

                if (text == "sample")
                {
                    text = "[H%]*([Y]/([X]+[Y]*[Z]))*(2.0141/1.0079)";
                }

                Console.WriteLine("------------");
                Console.WriteLine($"解析する式 : {text}");
                Console.WriteLine("------------");

                var node = parser.Parse(text);

                // 字句解析
                lexicalAnalyzer.Perform(node);

                // 変数値の存在をチェックする
                var attributeNames = node.GetAttributes().OrderBy(p => p);

                // 変数の代入
                foreach (var name in attributeNames)
                {
                    Console.Write($"{name} の値を入力 : ");
                    var value = double.Parse(Console.ReadLine());

                    var attribute = new AttributeValue(name, value);
                    expression.AddAttribute(attribute);
                }

                var result = expression.Execute(node);

                if (result != null)
                {
                    Console.WriteLine($"答え : {result}");
                }
                else
                {
                    Console.WriteLine("計算結果は表示できませんでした。式または値に誤りがあります。");
                }
            }


        }
    }
}
