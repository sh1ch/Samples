using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParserSample
{
    /// <summary>
    /// <see cref="Parser"/> クラスは、＊＊＊をサポートするためのクラスです。
    /// </summary>
    public class Parser
    {
        #region Properties

        /// <summary>
        /// 子ノードを表す文字を取得または設定します。
        /// </summary>
        public char ChildNodeChar { get; set; } = '#';

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="Parser"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public Parser()
        {

        }

        #endregion

        #region Public Methods

        public Node Parse(string text)
        {
            var root = new Node();
            var target = root;

            var c = text.Replace(" ", "");

            for (int i = 0; i < c.Length; i++)
            {
                switch (c[i])
                {
                    case '(':
                        target.Formula += ChildNodeChar;

                        // 子ノードを追加
                        var newNode = new Node();
                        target.Add(newNode);
                        target = newNode;
                        break;
                    case ')':
                        // 現在のノードを親に戻す
                        target = target.Parent;
                        break;
                    default:
                        target.Formula += c[i];
                        break;
                }
            }

            return root;
        }

        public double Eval(Node node)
        {
            List<string> ns; // 数式の値を表すテキスト
            List<char> os; // 演算子

            PerformLexicalAnalysis(node.Formula, out ns, out os);

            var numbers = new List<double>();
            int index = 0;

            // 値を取得する
            for (int i = 0; i < ns.Count; i++)
            {
                var number = 0.0D;

                if (ns[i] == ChildNodeChar.ToString())
                {
                    // () 式は、要素の再計算をする
                    number = Eval(node.ElementAt(index++));
                }
                else
                {
                    if (!double.TryParse(ns[i], out number))
                    {
                        throw new InvalidOperationException($"数値を表すテキストの変換に失敗しました。数値:{ns[i]}");
                    }
                }

                numbers.Add(number);
            }

            CalcMulDiv(ref numbers, ref os);
            var value = CalcAddSub(ref numbers, ref os);

            return value;
        }

        #endregion

        #region Private Methods

        private bool IsValueString(char c)
        {
            return char.IsDigit(c) || c == 'x' || c == 'X' || c == ChildNodeChar || c == '.';
        }

        private void PerformLexicalAnalysis(string str, out List<string> ns, out List<char> os)
        {
            var text = "";

            ns = new List<string>();
            os = new List<char>();

            for (var i = 0; i < str.Length; i++)
            {
                switch (str[i])
                {
                    case '+':
                    case '-':
                    case '*':
                    case '/':
                        ns.Add(text);
                        os.Add(str[i]);
                        text = "";
                        break;
                    default:
                        if (IsValueString(str[i]))
                        {
                            text += str[i];

                            if (i == str.Length - 1)
                            {
                                ns.Add(text);
                                text = "";
                            }
                        }
                        break;
                }
            }
        }

        private void CalcMulDiv(ref List<double> numbers, ref List<char> os)
        {
            for (var i = 0; i < os.Count;)
            {
                var l = numbers[i];
                var r = numbers[i + 1];

                if (os[i] == '*')
                {
                    numbers[i] = l * r;
                    numbers.RemoveAt(i + 1);
                    os.RemoveAt(i);
                }
                else if (os[i] == '/')
                {
                    numbers[i] = l / r;
                    numbers.RemoveAt(i + 1);
                    os.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        private double CalcAddSub(ref List<double> numbers, ref List<char> os)
        {
            var value = numbers[0];

            for (var i = 0; i < os.Count; i++)
            {
                if (os[i] == '+')
                {
                    value += numbers[i + 1];
                }
                else if (os[i] == '-')
                {
                    value -= numbers[i + 1];
                }
            }

            return value;
        }

        #endregion
    }
}
