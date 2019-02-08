using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionParserSample2.Math
{
    /// <summary>
    /// <see cref="Expression"/> クラスは、式を評価するクラスです。
    /// </summary>
    public class Expression
    {
        #region Properties

        private List<AttributeValue> _AttributeValues = new List<AttributeValue>();

        /// <summary>
        /// 設定された属性値の列挙子を取得します。
        /// </summary>
        public IEnumerable<AttributeValue> AttributeValues
        {
            get { return _AttributeValues; }
        }

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="Expression"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public Expression()
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 指定したノードを評価します。
        /// </summary>
        /// <param name="node">値を評価するノード。</param>
        public double? Execute(Node node)
        {
            var v = Evaluate(node);

            return v;
        }

        /// <summary>
        /// 式の評価に使用する指定した属性を追加します。
        /// </summary>
        /// <param name="attribute"></param>
        public void AddAttribute(AttributeValue attribute)
        {
            _AttributeValues.Add(attribute);
        }

        /// <summary>
        /// 式の評価に使用する指定した属性を追加します。
        /// </summary>
        /// <param name="attribute"></param>
        public void AddAttribute(IEnumerable<AttributeValue> attributes)
        {
            _AttributeValues.AddRange(attributes);
        }

        #endregion

        #region Private Methods

        private double? Evaluate(Node node)
        {
            var vs = node.Values; // 数式の値を表すテキスト
            var os = node.Operators; // 演算子

            var numbers = new List<double?>();
            int index = 0; 

            // 値を取得する
            for (int i = 0; i < vs.Count; i++)
            {
                double? number = 0.0D;
                var attribute = AttributeValues.SingleOrDefault(p => p.Name == vs[i]);

                if (attribute != null)
                {
                    // 属性値を割り当てる
                    number = attribute.Value;
                }
                else if (vs[i] == node.Parenthesis.ToString())
                {
                    // () 式は、要素の再計算をする
                    number = Evaluate(node.Childs[index++]);
                }
                else
                {
                    double temp = 0;
                    if (double.TryParse(vs[i], out temp))
                    {
                        number = temp;
                    }
                    else
                    {
                        number = null;
                        throw new InvalidOperationException($"テキストの変換に失敗しました。数値:{vs[i]}");
                    }
                }

                numbers.Add(number);
            }

            CalcMulDiv(ref numbers, ref os);
            var value = CalcAddSub(ref numbers, ref os);

            return value;
        }

        private void CalcMulDiv(ref List<double?> numbers, ref List<char> os)
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

        private double? CalcAddSub(ref List<double?> numbers, ref List<char> os)
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
