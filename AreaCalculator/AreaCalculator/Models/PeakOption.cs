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
    /// <see cref="PeakOption"/> クラスは、ピークに関するオプション情報を表すクラスです。
    /// </summary>
    public class PeakOption : INotifyPropertyChanged
    {
        #region Properties

        private double _f;

        /// <summary>
        /// シンメトリー係数に関する項 f の値を取得します。
        /// </summary>
        public double f
        {
            get { return _f; }
            private set
            {
                _f = value;
                PropertyChanged.Raise(this, nameof(f));
            }
        }

        private double _W_005;

        /// <summary>
        /// シンメトリー係数に関する項 W の値を取得します。
        /// </summary>
        public double W_005
        {
            get { return _W_005; }
            set
            {
                _W_005 = value;
                PropertyChanged.Raise(this, nameof(W_005));
            }
        }

        private double _SymmetryFactor;

        /// <summary>
        /// シンメトリー係数を取得します。
        /// </summary>
        public double SymmetryFactor
        {
            get { return _SymmetryFactor; }
            private set
            {
                _SymmetryFactor = value;
                PropertyChanged.Raise(this, nameof(SymmetryFactor));
            }
        }

        private double _W_05;

        /// <summary>
        /// 理論段数に関する項 W の値を取得します。
        /// </summary>
        public double W_05
        {
            get { return _W_05; }
            set
            {
                _W_05 = value;
                PropertyChanged.Raise(this, nameof(W_05));
            }
        }

        private double _TheoreticalPlateNumber;

        /// <summary>
        /// 理論段数を取得します。
        /// </summary>
        public double TheoreticalPlateNumber
        {
            get { return _TheoreticalPlateNumber; }
            private set
            {
                _TheoreticalPlateNumber = value;
                PropertyChanged.Raise(this, nameof(TheoreticalPlateNumber));
            }
        }

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="PeakOption"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public PeakOption()
        {

        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Methods

        public void UpdateSymmetryFactor(IEnumerable<DataPoint> points, PeakResult result)
        {
            var startSeconds = result.StartPoint.X;
            var endSeconds = result.EndPoint.X;
            var peakPoints = points.Where(p => p.X >= startSeconds && p.X <= endSeconds);

            var lf = 0.0D;
            var w = 0.0D;
            var sf = 0.0D;

            var a = result.BaseLine.Item1;
            var b = result.BaseLine.Item2;

            if ((result.IsEnabled) && ((peakPoints?.Count() ?? 0) > 0))
            {
                var topPoint = result.TopPoint;
                var leftPoints = peakPoints.Where(p => p.X < topPoint.X);
                var rightPoints = peakPoints.Where(p => p.X > topPoint.X);

                var h = (topPoint.Y - (topPoint.X * a + b)) * 0.05D; // 0.05h

                if (((leftPoints?.Count() ?? 0) > 0) && ((rightPoints?.Count() ?? 0) > 0) && h > 0)
                {
                    var nearestLeft = NearestY(leftPoints, h).MaxBy(p => p.X);
                    var nearestRight = NearestY(rightPoints, h).MinBy(p => p.X);

                    lf = topPoint.X - nearestLeft.X;
                    w = nearestRight.X - nearestLeft.X;
                    sf = w / (2 * lf);
                }
            }

            f = lf;
            W_005 = w;
            SymmetryFactor = sf;
        }

        public void UpdateTheoreticalPlateNumber(IEnumerable<DataPoint> points, PeakResult result)
        {
            var startSeconds = result.StartPoint.X;
            var endSeconds = result.EndPoint.X;
            var peakPoints = points.Where(p => p.X >= startSeconds && p.X <= endSeconds);

            var w = 0.0D;
            var tp = 0.0D;
            var tr = 0.0D;

            var a = result.BaseLine.Item1;
            var b = result.BaseLine.Item2;

            if ((result.IsEnabled) && ((peakPoints?.Count() ?? 0) > 0))
            {
                var topPoint = result.TopPoint;
                var leftPoints = peakPoints.Where(p => p.X < topPoint.X);
                var rightPoints = peakPoints.Where(p => p.X > topPoint.X);

                var h = (topPoint.Y - (topPoint.X * a + b)) * 0.5D; // 0.5h

                if (((leftPoints?.Count() ?? 0) > 0) && ((rightPoints?.Count() ?? 0) > 0) && h > 0)
                {
                    var nearestLeft = NearestY(leftPoints, h).MaxBy(p => p.X);
                    var nearestRight = NearestY(rightPoints, h).MinBy(p => p.X);

                    tr = topPoint.X - 0.0D; // 保持時間
                    w = nearestRight.X - nearestLeft.X;
                    tp = 5.54 * (Math.Pow(tr / w, 2));
                }
            }

            W_05 = w;
            TheoreticalPlateNumber = tp;
        }

        #endregion

        #region Private Methods

        public IEnumerable<DataPoint> NearestY(IEnumerable<DataPoint> points, double y)
        {
            var minY = points.Min(p => Math.Abs(p.Y - y));
            return points.Where(p => Math.Abs(p.Y - y) == minY);
        }

        #endregion

    }
}
