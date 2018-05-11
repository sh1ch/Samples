using AreaCalculator.Models.SignalConverter;
using HeritageFramework;
using HeritageFramework.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreaCalculator.Models
{
    /// <summary>
    /// <see cref="DataPoint"/> クラスは、信号のデータを表現するクラスです。
    /// </summary>
    [DebuggerDisplay("X={X}, Y={Y}, NextSlope={NextSlope}")]
    public class DataPoint
    {
        #region Properties

        /// <summary>
        /// 時間を表す値を取得または設定します。
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// 信号強度の値を取得または設定します。
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// タグの値を取得または設定します。
        /// </summary>
        public object Tag { get; set; } = null;

        /// <summary>
        /// 連続するデータ ポイントの前のデータを取得または設定します。
        /// </summary>
        public DataPoint Previous { get; set; }

        /// <summary>
        /// 連続するデータ ポイントの次のデータを取得または設定します。
        /// </summary>
        public DataPoint Next { get; set; }

        /// <summary>
        /// 前のデータとの傾き m の値を取得します。
        /// </summary>
        public double PreviousSlope
        {
            get { return -GetSlope(Previous); }
        }

        /// <summary>
        /// 次のデータとの傾き m の値を取得します。
        /// </summary>
        public double NextSlope
        {
            get { return GetSlope(Next); }
        }

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="DataPoint"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="sec">横軸を指定する秒数。</param>
        /// <param name="signal">縦軸を指定する信号強度。</param>
        public DataPoint(double sec, double signal)
        {
            X = sec;
            Y = signal;
        }

        /// <summary>
        /// <see cref="DataPoint"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="sec">横軸を指定する秒数。</param>
        /// <param name="signal">縦軸を指定する信号強度。</param>
        /// <param name="converter">信号強度を表示値に変換するコンバーター。</param>
        public DataPoint(double sec, double signal, ISignalConverter converter)
        {
            X = sec;
            Y = converter?.Parse(signal) ?? signal;
        }

        /// <summary>
        /// このオブジェクトを原点とし、指定したポイントとの２点間の傾き (m) を取得します。
        /// </summary>
        /// <param name="point">位置を表すデータ ポイント。</param>
        /// <returns>傾きの値。</returns>
        public double GetSlope(DataPoint point)
        {
            if (point == null) throw new ArgumentNullException("指定された引数は null です。");

            return (point.Y - Y) / (point.X - X);
        }

        #endregion
    }
}
