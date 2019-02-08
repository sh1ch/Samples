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
    /// <see cref="Node"/> クラスは、式を表すノードクラスです。
    /// </summary>
    public class Node
    {
        #region Properties

        /// <summary>
        /// 括弧の式を表す代替文字を取得します。
        /// </summary>
        public char Parenthesis { get; } = '#';

        /// <summary>
        /// 数式を表すテキストを取得または設定します。
        /// </summary>
        public string Formula { get; set; }

        /// <summary>
        /// 親ノードを取得または設定します。
        /// </summary>
        public Node Parent { get; set; }

        /// <summary>
        /// 子ノードの列挙子を取得します。
        /// </summary>
        public List<Node> Childs { get; } = new List<Node>();

        /// <summary>
        /// ノードの持つ値の列挙子を取得または設定します。
        /// </summary>
        public List<string> Values { get; set; } = new List<string>();

        /// <summary>
        /// ノードの持つ演算子の列挙子を取得または設定します。
        /// </summary>
        public List<char> Operators { get; set; } = new List<char>();

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="Node"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public Node()
        {

        }

        /// <summary>
        /// <see cref="Node"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="parenthesis">括弧式の代替文字。</param>
        public Node(char parenthesis)
        {
            Parenthesis = parenthesis;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// <see cref="System.Console"/> にノードが持つ数式構造を表示します。
        /// </summary>
        public void Print()
        {
            Print(0, this);
        }

        /// <summary>
        /// ノードが持つ属性値 (変数値) の列挙子を取得します。
        /// </summary>
        /// <param name="isRecursion">子ノードを再帰的に解析するかどうかを示す値。</param>
        /// <returns></returns>
        public IEnumerable<string> GetAttributes(bool isRecursion = true)
        {
            var attributes = GetAttributes(this, isRecursion).Distinct();

            return attributes;
        }

        #endregion

        #region Private Methods

        private void Print(int depth, Node node)
        {
            var prefix = "";
            var cursor = "->";

            if (depth > 0)
            {
                prefix = string.Concat(Enumerable.Repeat($"{cursor} ", depth));
            }

            Console.WriteLine(prefix + node.Formula);

            foreach (var c in node.Childs)
            {
                Print(depth + 1, c);
            }
        }

        public IEnumerable<string> GetAttributes(Node node, bool isRecursion = true)
        {
            var attributes = new List<string>();

            foreach (var value in node.Values)
            {
                if (AttributeValue.IsAttribute(value))
                {
                    attributes.Add(value);
                }
            }

            if (isRecursion)
            {
                foreach (var child in node.Childs)
                {
                    attributes.AddRange(GetAttributes(child, isRecursion));
                }
            }

            return attributes;
        }

        #endregion
    }
}
