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
    /// <see cref="LexicalAnalysis"/> クラスは、数式を表すテキストの字句解析をサポートするためのクラスです。
    /// </summary>
    public class LexicalAnalysis
    {
        #region Properties

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="LexicalAnalysis"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public LexicalAnalysis()
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 指定したノードの字句解析をします。
        /// </summary>
        /// <param name="node">解析をするノード。</param>
        /// <param name="isRecursion">再帰的に解析をするかどうかを示す値。</param>
        public void Perform(Node node, bool isRecursion = true)
        {
            Analyze(node);

            if (isRecursion)
            {
                foreach (var child in node.Childs)
                {
                    Perform(child, isRecursion);
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 指定したノードの持つ数式のテキストを字句解析します。
        /// </summary>
        /// <param name="node">解析するノード。</param>
        private void Analyze(Node node)
        {
            var text = "";
            var formula = node.Formula;
            var parenthesis = node.Parenthesis;

            var values = new List<string>();
            var operators = new List<char>();

            for (var i = 0; i < formula.Length; i++)
            {
                switch (formula[i])
                {
                    case '+':
                    case '-':
                    case '*':
                    case '/':
                        values.Add(text);
                        operators.Add(formula[i]);
                        text = "";
                        break;
                    default:
                        text += formula[i];

                        if (i == formula.Length - 1)
                        {
                            values.Add(text);
                            text = "";
                        }
                        break;
                }
            }

            node.Values = values;
            node.Operators = operators;
        }

        /// <summary>
        /// 指定した文字が値であるかどうかを示す値を取得します。
        /// <para>
        /// 値は 0-9 までの数字 "." 指定した引数の値のとき true。
        /// それ以外は false 。
        /// </para>
        /// </summary>
        /// <param name="c">確認する文字。</param>
        /// <param name="args">値とみなす値。</param>
        /// <returns>値のとき true 。それ以外のとき false 。</returns>
        [Obsolete]
        private bool IsValueString(char c, params char[] args)
        {
            foreach (var arg in args)
            {
                if (c == arg) return true;
            }

            return char.IsDigit(c) || c == '.';
        }

        #endregion
    }
}
