using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XorshiftRandom
{
    /// <summary>
    /// <see cref="Random"/> クラスは、Xorshift RNGs による疑似乱数ジェネレーターです。
    /// <para>
    /// 静的に乱数を運用するために使用します。通常、エフェクトなど再現性を考慮しない要素に利用します。
    /// </para>
    /// </summary>
    public static class Random
    {
        #region Fields

        private static RandomState _State = new RandomState();

        #endregion

        #region Public Methods

        /// <summary>
        /// 疑似乱数ジェネレーターの状態を決めるシード値を設定します。
        /// </summary>
        /// <param name="seed">擬似乱数系列の開始値を計算するために使用するシード値。</param>
        public static void SetSeed(long seed)
        {
            _State.SetSeed((ulong)seed);
        }

        /// <summary>
        /// 疑似乱数ジェネレーターの状態を決めるシード値を設定します。
        /// </summary>
        /// <param name="seed">擬似乱数系列の開始値を計算するために使用するシード値。</param>
        public static void SetSeed(ulong seed)
        {
            _State.SetSeed(seed);
        }

        /// <summary>
        /// 指定した範囲のランダムな数値を取得します。（指定した値を含む）
        /// </summary>
        /// <param name="minValue">最小値。</param>
        /// <param name="maxValue">最大値。</param>
        /// <returns>疑似乱数値。</returns>
        public static int Range(int minValue, int maxValue)
        {
            return _State.Range(minValue, maxValue);
        }

        /// <summary>
        /// 指定した範囲のランダムな数値を取得します。（指定した値を含む）
        /// </summary>
        /// <param name="minValue">最小値。</param>
        /// <param name="maxValue">最大値。</param>
        /// <returns>疑似乱数値。</returns>
        public static double Range(double minValue = 0.0D, double maxValue = 1.0D)
        {
            return _State.Range(minValue, maxValue);
        }

        /// <summary>
        /// 疑似乱数を取得します。
        /// <para>
        /// 乱数の範囲は、0 ≦ w ≦ (2^64) -1 です。（値を含む）
        /// </para>
        /// </summary>
        /// <returns>疑似乱数値。</returns>
        public static ulong GetNext()
        {
            return _State.GetNext();
        }

        #endregion
    }
}
