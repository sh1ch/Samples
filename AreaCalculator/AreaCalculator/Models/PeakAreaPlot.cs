using HeritageFramework;
using OxyPlot;
using OxyPlot.Series;
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
    /// <see cref="PeakAreaPlot"/> クラスは、ピークをプロットに表示するクラスです。
    /// </summary>
    public class PeakAreaPlot : PlotBase
    {
#if DEBUG
        private const int AXIS_X_MIN_VALUE = 0;
        private const int AXIS_X_MAX_VALUE = 60 * 6;
        private const int AXIS_Y_MIN_VALUE = -100;
        private const int AXIS_Y_MAX_VALUE = 1500;
#else
        private const int AXIS_X_MIN_VALUE = 0;
        private const int AXIS_X_MAX_VALUE = 60 * 6;
        private const int AXIS_Y_MIN_VALUE = 0;
        private const int AXIS_Y_MAX_VALUE = 1500;
#endif

        #region Properties

        public AreaSeries Series { get; } = new AreaSeries()
        {
            TrackerFormatString = "{1} : {2:mm\\:ss\\.f}\n{3} : {4:0.000}\n{Tag}",
        };

        public ScatterSeries ScatterSeries { get; } = new ScatterSeries()
        {
            MarkerType = MarkerType.Circle,
            MarkerSize = 5,
            MarkerFill = OxyColors.MidnightBlue,
            TrackerFormatString = "{Tag}\n{1} : {2:mm\\:ss\\.f}\n{3} : {4:0.000}",
        };

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="PeakAreaPlot"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public PeakAreaPlot()
        {
            // 軸の初期化
            InitializeAxisX(AXIS_X_MIN_VALUE, AXIS_X_MIN_VALUE, AXIS_X_MAX_VALUE, AXIS_X_MAX_VALUE, 20);
            InitializeAxisY(AXIS_Y_MIN_VALUE, AXIS_Y_MIN_VALUE, AXIS_Y_MAX_VALUE, AXIS_Y_MAX_VALUE, 50);

            PlotModel.Axes.Add(AxisX);
            PlotModel.Axes.Add(AxisY);
            PlotModel.Series.Add(Series);
            PlotModel.Series.Add(ScatterSeries);

            PlotModel.MouseMove += (sender, args) =>
            {
                var lp = OxyPlot.Axes.Axis.InverseTransform(args.Position, AxisX, AxisY);
                var np = Series.GetNearestPoint(args.Position, false)?.DataPoint ?? new OxyPlot.DataPoint(0, 0);

                var loosePoint = new DataPoint((int)lp.X, (int)lp.Y);
                var nearestPoint = new DataPoint(np.X, np.Y);

                MouseMove?.Invoke(this, GenericEventArgs.Create(new PositionEventArgs(loosePoint, nearestPoint)));
            };

        }

        #endregion

        #region Events

        public event EventHandler<GenericEventArgs<PositionEventArgs>> MouseMove;

        #endregion

        #region Public Methods

        public void InitializeSeries1(IEnumerable<DataPoint> points)
        {
            ClearSeries();

            foreach (var p in  points)
            {
                Series.Points.Add(new OxyPlot.DataPoint(p.X, p.Y));
                Series.Points2.Add(new OxyPlot.DataPoint(p.X, p.Y));
            }
        }

        public void InitializeSeries2(IEnumerable<DataPoint> points, PeakResult result)
        {
            Series.Points2.Clear();

            // ピークが認識できていないときは、エリア表示を使用しない
            if (result.IsEnabled == false)
            {
                foreach (var p in points)
                {
                    Series.Points2.Add(new OxyPlot.DataPoint(p.X, p.Y));
                }
                return;
            }

            var startSeconds = result.StartPoint.X;
            var endSeconds = result.EndPoint.X;

            var beforePoints = points.Where(p => p.X < startSeconds);
            var afterPoints = points.Where(p => p.X > endSeconds);
            var peakPoints = points.Where(p => p.X >= startSeconds && p.X <= endSeconds);

            // ピーク開始前の点
            if ((beforePoints?.Count() ?? 0) > 0)
            {
                foreach (var p in beforePoints)
                {
                    Series.Points2.Add(new OxyPlot.DataPoint(p.X, p.Y));
                }
            }

            // ピーク中の点
            if ((peakPoints?.Count() ?? 0) > 0)
            {
                var x = result.BaseLine.Item1;
                var y = result.BaseLine.Item2;

                foreach (var p in peakPoints)
                {
                    double px = 0; double py = 0;
                    px = p.X;
                    py = p.X * x + y;

                    // 実際の位置がベースラインよりも低いときは、実際の点の位置を優先する
                    if (p.Y < py)
                    {
                        py = p.Y;
                    }

                    Series.Points2.Add(new OxyPlot.DataPoint(px, py));
                }
            }

            // ピーク終了後の点
            if ((afterPoints?.Count() ?? 0) > 0)
            {
                foreach (var p in afterPoints)
                {
                    Series.Points2.Add(new OxyPlot.DataPoint(p.X, p.Y));
                }
            }
        }

        public void UpdatePeakPoints(PeakResult result)
        {
            // 現在のポイントをクリア
            ScatterSeries.Points.Clear();

            // ピークとして認められていないとき、追加しない
            if (result.IsEnabled == false)
            {
                return;
            }

            // 最高点を求める
            var topPoint = result.TopPoint;

            if (topPoint != null)
            {
                ScatterSeries.Points.Add(new ScatterPoint(topPoint.X, topPoint.Y, tag: "ピークの頂点"));
            }

            var startPoint = result.StartPoint;
            var endPoint = result.EndPoint;

            if (startPoint != null)
            {
                ScatterSeries.Points.Add(new ScatterPoint(startPoint.X, startPoint.Y, tag: "ピークの開始点"));
            }

            if (endPoint != null)
            {
                ScatterSeries.Points.Add(new ScatterPoint(endPoint.X, endPoint.Y, tag: "ピークの終了点"));
            }
        }

        public void ClearSeries()
        {
            Series.Points.Clear();
            Series.Points2.Clear();
            ScatterSeries.Points.Clear();
        }

        public override void SaveCsvFile(string filePath)
        {
            throw new InvalidOperationException();
        }

        #endregion
    }
}
