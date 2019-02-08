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
    /// <see cref="AttributeValue"/> クラスは、属性値 (変数値) を表現するクラスです。
    /// </summary>
    public class AttributeValue
    {
        #region Properties

        /// <summary>
        /// 属性の名前を取得します。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 属性の値を取得します。
        /// </summary>
        public double? Value { get; private set; }

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="AttributeValue"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="name">属性の名前。</param>
        /// <param name="value">属性の値。</param>
        public AttributeValue(string name, double? value)
        {
            Name = name;
            Value = value;
        }

        #endregion

        #region Public Methods

        public static bool IsAttribute(string value)
        {
            if (string.IsNullOrEmpty(value)) return false;

            return (value[0] == '[' && value[value.Length - 1] == ']');
        }

        /// <summary>
        /// 指定した属性の名前が一致するかどうかを示す値を取得します。
        /// </summary>
        /// <param name="name">属性の名前。</param>
        /// <returns>一致するとき true 。それ以外のとき false 。</returns>
        public bool IsMatch(string name)
        {
            if (IsAttribute(name))
            {
                if (Name == name)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
