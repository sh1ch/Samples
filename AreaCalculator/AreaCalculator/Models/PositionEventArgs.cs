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
    /// <see cref="PositionEventArgs"/> クラスは、プロットの位置情報に関するイベントの値を提供するクラスです。
    /// </summary>
    public class PositionEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// おおよその選択位置を取得します。
        /// </summary>
        public DataPoint LooseSelectedDataPoint { get; }

        /// <summary>
        /// 最も近いデータ位置を取得します。
        /// </summary>
        public DataPoint NearestDataPoint { get; }

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="PositionEventArgs"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="loosePoint">おおよその選択位置。</param>
        /// <param name="nearestPoint">最も近いデータ位置。</param>
        public PositionEventArgs(DataPoint loosePoint, DataPoint nearestPoint)
        {
            LooseSelectedDataPoint = loosePoint;
            NearestDataPoint = nearestPoint;
        }

        #endregion
    }
}
