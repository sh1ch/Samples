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
    /// <see cref="Node"/> クラスは、＊＊＊をサポートするためのクラスです。
    /// </summary>
    public class Node
    {
        #region Properties

        /// <summary>
        /// 数式を表すテキストを取得または設定します。
        /// </summary>
        public string Formula { get; set; }

        /// <summary>
        /// 親ノードを取得します。
        /// </summary>
        public Node Parent { get; private set; }

        private List<Node> _Childs = new List<Node>();

        /// <summary>
        /// 子ノードの列挙子を取得します。
        /// </summary>
        public IEnumerable<Node> Childs
        {
            get { return _Childs; }
        }

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="Node"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public Node()
        {

        }

        #endregion

        #region Public Methods

        public void Add(Node child)
        {
            if (child == null) throw new ArgumentNullException();

            child.Parent = this;
            _Childs.Add(child);
        }

        public Node ElementAt(int i)
        {
            return _Childs.ElementAt(i);
        }

        /// <summary>
        /// <see cref="System.Console"/> にノードが持つ数式構造を表示します。
        /// </summary>
        public void Print()
        {
            Print(0, this);
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

        #endregion
    }
}
