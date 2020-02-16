using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptHashSample
{
    /// <summary>
    /// <see cref="Hash" /> クラスは、任意のテキスト値から固定長のハッシュ値に変換するクラスです。
    /// <para>
    /// salt 値は 8 バイト以上、演算の反復処理回数は 1 回以上を指定する必要があります。
    /// </para>
    /// </summary>
    public class Hash
    {
        #region Fields

        private int _Iteration = 1024;

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="Hash"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public Hash() { }

        #endregion

        #region Public Methods

        /// <summary>
        /// PBKDF2 に基づくハッシュ値を取得します。
        /// </summary>
        /// <param name="password">ハッシュ値を得るパスワード。</param>
        /// <param name="salt">ハッシュ値を派生させるために使用する salt の値。</param>
        /// <param name="size">生成するハッシュ値のバイト数</param>
        /// <returns>ハッシュ値を格納したバイト配列。</returns>
        /// <exception cref="ArgumentNullException">指定したパスワードは null または空白です。</exception>
        /// <exception cref="ArgumentNullException">指定した　salt は null または空白です。</exception>
        /// <exception cref="ArgumentException">指定された salt のサイズが 8 バイト未満です。</exception>
        /// <exception cref="InvalidOperationException">反復処理回数は 1 回以上を指定してください。</exception>
        public byte[] GetPbkdf2(string password, string salt, int size = 32)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("指定したパスワードは null または空白です。");
            }

            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(salt))
            {
                throw new ArgumentNullException("指定した　salt は null または空白です。");
            }

            var saltBytes = Encoding.UTF8.GetBytes(salt);

            if ((saltBytes?.Length ?? 0) <= 8)
            {
                throw new ArgumentException("指定された salt のサイズが 8 バイト未満です。");
            }

            if (_Iteration <= 0)
            {
                throw new InvalidOperationException("反復処理回数は 1 回以上を指定してください。");
            }

            return new Rfc2898DeriveBytes(password, saltBytes, _Iteration)?.GetBytes(size);
        }

        /// <summary>
        /// 演算の反復処理回数を変更します。
        /// </summary>
        /// <param name="iteration">演算の反復処理回数。</param>
        /// <exception cref="ArgumentOutOfRangeException">反復処理回数の回数は 1 回以上を指定してください。</exception>
        public void ChangeIteration(int iteration)
        {
            if (iteration <= 0)
            {
                throw new ArgumentOutOfRangeException("反復処理回数の回数は 1 回以上を指定してください。");
            }

            _Iteration = iteration;
        }

        #endregion

    }
}
