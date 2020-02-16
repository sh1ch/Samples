using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptHashSample
{
    /// <summary>
    /// <see cref="HashGenerator"/> クラスは、パスワードのハッシュ値を生成するクラスです。
    /// </summary>
    public class HashGenerator
    {
        #region Fields

        private static string _Salt = "qqWsf2yqy7vuZ9BDdwv0mlLnbhoTS6vw9Pr0NU5tD1aneMyydEv82lIxykeepfZ8";
        private static Hash _Hash = new Hash();

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="HashGenerator"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public HashGenerator() { }

        #endregion

        #region Public Methods

        /// <summary>
        /// ハッシュ値を派生させるために使用する salt 値を変更します。
        /// <para>
        /// salt 値は 64 文字以上を設定する必要があります。
        /// </para>
        /// </summary>
        /// <param name="salt">変更する salt 値。</param>
        /// <exception cref="ArgumentException">salt の文字数は 64 文字以上を入力してください。</exception>
        public static void ChangeSalt(string salt)
        {
            if (salt.Length < 64)
            {
                throw new ArgumentException("salt の文字数は 64 文字以上を入力してください。");
            }

            _Salt = salt;
        }

        /// <summary>
        /// 指定したパスワードのハッシュ値を表すテキストを生成します。
        /// </summary>
        /// <param name="password">パスワードを表すテキスト。</param>
        /// <param name="length">ハッシュ値を表すテキストの文字数。</param>
        /// <returns>ハッシュ値を表すテキスト。</returns>
        /// <exception cref="ArgumentNullException">パスワードのテキストが空白です。</exception>
        /// <exception cref="ArgumentOutOfRangeException">ハッシュ値の長さは 1 以上を指定する必要があります。</exception>
        /// <exception cref="InvalidOperationException">ハッシュ値の生成に失敗しました。</exception>
        public static string Generate(string password, int length = 64)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("パスワードのテキストが空白です。");
            }

            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException("ハッシュ値の長さは 1 以上を指定する必要があります。");
            }
            
            var hash = _Hash.GetPbkdf2(password, _Salt, length / 2);

            if (hash == null || hash.Length != (int)(length / 2))
            {
                throw new InvalidOperationException("ハッシュ値の生成に失敗しました。");
            }

            var hashText = string.Join("", hash.Select(p => Convert.ToByte(p).ToString("x2")));

            // 奇数の場合は末尾に既定の文字 0 を追加する
            if (length % 2 != 0)
            {
                hashText += "0";
            }

            return hashText;
        }

        #endregion
    }
}
