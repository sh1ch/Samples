using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FixedDecimalPointTextBoxSample
{
    /// <summary>
    /// <see cref="NumericalTextStatus"/> クラスは、数値を表すテキストの状態を表すクラスです。
    /// </summary>
    public class NumericalTextStatus
    {
        #region Properties

        /// <summary>
        /// （数値を表す）テキストを取得または設定します。
        /// </summary>
        public string Raw { get; set; }

        /// <summary>
        /// <see cref="Raw"/> の文字数を取得します。
        /// </summary>
        public int Length => Raw.Length;

        /// <summary>
        /// 整数部分の数値を表すテキストを取得します。
        /// </summary>
        public string IntegerPartText
        {
            get
            {
                string text;

                if (HasDecimal)
                {
                    // 整数部だけ取得
                    text = Raw.Substring(0, Raw.IndexOf('.'));
                }
                else
                {
                    // すべて数値
                    text = Raw;
                }

                if (!int.TryParse(text, out _)) { return ""; }

                return text;
            }
        }

        /// <summary>
        /// 小数部分の数値を表すテキストを取得します。
        /// </summary>
        public string DecimalPartText
        {
            get
            {
                string text;

                if (HasDecimal)
                {
                    // 整数部だけ取得
                    var index = Raw.IndexOf('.');

                    // "." を含まない
                    text = Raw.Substring(index + 1, Raw.Length - index - 1);
                }
                else
                {
                    // 小数を持たない
                    return "";
                }

                if (!int.TryParse(text, out _)) { return ""; }
                
                return text;
            }
        }

        /// <summary>
        /// 小数を表す数値を持つかどうかを示す値を取得します。
        /// </summary>
        public bool HasDecimal => Raw.Contains(".");

        /// <summary>
        /// 小数を表す開始位置の０から始まるインデックス値を取得します。
        /// </summary>
        public int IndexOfDecimal
        {
            get
            {
                if (!HasDecimal)
                {
                    return -1;
                }

                var index = Raw.IndexOf('.');

                return index;
            }
        }

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="NumericalTextStatus"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public NumericalTextStatus(string text)
        {
            Raw = text;
        }

        #endregion

        #region Public Methods

        public string SetDecimals(int length)
        {
            var tail = "";

            if (length == 0)
            {
                if (HasDecimal)
                {
                    Raw = Raw.Substring(0, Raw.IndexOf('.'));
                    return Raw;
                }
            }

            if (!HasDecimal)
            {
                // 小数を持たない
                tail += ".";
                tail += string.Concat(Enumerable.Repeat("0", length));

                Raw += tail;

                return Raw;
            }
            else
            {
                // 小数を持つ
                if (length == DecimalPartText.Length) return Raw;

                if (length > DecimalPartText.Length)
                {
                    tail += string.Concat(Enumerable.Repeat("0", length - DecimalPartText.Length));
                    Raw += tail;
                }
                else if(length < DecimalPartText.Length)
                {
                    Raw = Raw.Substring(0, Raw.Length - (DecimalPartText.Length - length));
                }
            }

            return Raw;
        }

        public string Delete(int index)
        {
            if (Raw.Length == 0) return Raw;

            // 小数は置換
            if (index > 0 && index >= Raw.Length) index -= 1;

            if (index > 0)
            {
                var subChar = Raw.Substring(index, 1);

                if (subChar == ".") return Raw;
            }

            if (IncludeInteger(index))
            {
                // 整数は消せばいい
                Raw = Raw.Remove(index, 1);

                if (IntegerPartText == "")
                {
                    Raw = Raw.Insert(0, '0'.ToString());
                }
            }
            else if (IncludeDecimal(index))
            {
                Raw = Raw.Remove(index, 1).Insert(index, "0");
            }

            return Raw;
        }

        public string InsertOrReplace(int digit, int index)
        {
            if (digit < 0 || digit > 9)
            {
                throw new ArgumentOutOfRangeException("変更する数値は 0-9 の範囲です。");
            }

            return InsertOrReplace((char)(digit + (int)'0'), index);
        }

        public string InsertOrReplace(char digit, int index)
        {
            if (IncludeInteger(index))
            {
                // 整数は追加
                Raw = Raw.Insert(index, digit.ToString());
            }
            else if (IncludeDecimal(index))
            {
                // 小数は置換
                if (index >= Raw.Length) index -= 1;

                Raw = Raw.Remove(index, 1).Insert(index, digit.ToString());
            }

            // 整数値に不要な数値があるときは、削除する
            TrimInteger();

            return Raw;
        }

        #endregion

        #region Private Method

        private void TrimInteger()
        {
            int temp;

            if (HasDecimal)
            {
                var point = Raw.IndexOf('.');

                var integerText = Raw.Substring(0, point);
                var decimalText = Raw.Substring(point, Raw.Length - point); // 小数 + "."

                if (int.TryParse(integerText, out temp))
                {
                    Raw = temp.ToString() + decimalText;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            else 
            {
                if (int.TryParse(Raw, out temp))
                {
                    Raw = temp.ToString();
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        private bool IncludeInteger(int index)
        {
            if (!HasDecimal) return true;

            var point = Raw.IndexOf('.');

            return (index <= point);
        }

        private bool IncludeDecimal(int index)
        {
            if (!HasDecimal) return false;

            var point = Raw.IndexOf('.');

            return (index > point);
        }

        #endregion
    }
}
