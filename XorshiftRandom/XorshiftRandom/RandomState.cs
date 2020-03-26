using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XorshiftRandom
{
    /// <summary>
    /// <see cref="RandomState"/> クラスは、Xorshift RNGs による疑似乱数ジェネレーターです。
    /// <para>
    /// 個別に乱数を運用するために使用します。通常、乱数の再現性を必要とするコンテンツに利用します。
    /// </para>
    /// </summary>
    public class RandomState
    {
        #region Fields

        private ulong _x = 123456789U;
        private ulong _y = 362436069U;
        private ulong _z = 521288629U;
        private ulong _w = 88675123U;

        private static ulong MIN_VALUE = ulong.MinValue;
        private static ulong MAX_VALUE = ulong.MaxValue;

        #endregion

        #region Properties



        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="RandomState"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public RandomState() : this(88675123U) { }

        /// <summary>
        /// <see cref="RandomState"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="seed">擬似乱数系列の開始値を計算するために使用するシード値。</param>
        public RandomState(int seed)
        {

        }

        /// <summary>
        /// <see cref="RandomState"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="seed">擬似乱数系列の開始値を計算するために使用するシード値。</param>
        public RandomState(uint seed)
        {

        }

        /// <summary>
        /// <see cref="RandomState"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="seed">擬似乱数系列の開始値を計算するために使用するシード値。</param>
        public RandomState(long seed)
        {

        }

        /// <summary>
        /// <see cref="RandomState"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="seed">擬似乱数系列の開始値を計算するために使用するシード値。</param>
        public RandomState(ulong seed)
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 疑似乱数ジェネレーターの状態を決めるシード値を設定します。
        /// </summary>
        /// <param name="seed">擬似乱数系列の開始値を計算するために使用するシード値。</param>
        public void SetSeed(long seed)
        {
            SetSeed((ulong)seed);
        }

        /// <summary>
        /// 疑似乱数ジェネレーターの状態を決めるシード値を設定します。
        /// </summary>
        /// <param name="seed">擬似乱数系列の開始値を計算するために使用するシード値。</param>
        public void SetSeed(ulong seed)
        {
            _x = seed;
            _y = _x * 3266489917U + 1;
            _z = _y * 3266489917U + 1;
            _w = _z * 3266489917U + 1;
        }

        /// <summary>
        /// 指定した範囲のランダムな数値を取得します。（指定した値を含む）
        /// </summary>
        /// <param name="minValue">最小値。</param>
        /// <param name="maxValue">最大値。</param>
        /// <returns>疑似乱数値。</returns>
        public int Range(int minValue, int maxValue)
        {
            return Convert.ToInt32(Range((double)minValue, (double)maxValue));
        }

        /// <summary>
        /// 指定した範囲のランダムな数値を取得します。（指定した値を含む）
        /// </summary>
        /// <param name="minValue">最小値。</param>
        /// <param name="maxValue">最大値。</param>
        /// <returns>疑似乱数値。</returns>
        public double Range(double minValue = 0.0D, double maxValue = 1.0D)
        {
            var w = ToNormalize(GetNext());

            return minValue + (w * (maxValue - minValue));
        }

        /// <summary>
        /// 疑似乱数を取得します。
        /// <para>
        /// 乱数の範囲は、0 ≦ w ≦ (2^64) -1 です。（値を含む）
        /// </para>
        /// </summary>
        /// <returns>疑似乱数値。</returns>
        public ulong GetNext()
        {
            ulong t = _x ^ (_x << 11);

            // ulong が 64 bit まで
            _x = _y;
            _y = _z;
            _z = _w;
            _w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8));

            return _w;
        }

        #endregion

        #region Private Methods

        private double ToNormalize(ulong w)
        {
            var max = MAX_VALUE;
            var min = MIN_VALUE;

            return ((double)(w - MIN_VALUE)) / ((double)(MAX_VALUE - MIN_VALUE));
        }

        #endregion
    }
}
