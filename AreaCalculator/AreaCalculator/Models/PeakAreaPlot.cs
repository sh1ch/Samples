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
            TrackerFormatString = "{1} : {2:mm\\:ss}\n{3} : {4:0.###}\n{Tag}",
        };

        public ScatterSeries ScatterSeries { get; } = new ScatterSeries()
        {
            MarkerType = MarkerType.Circle,
            MarkerSize = 5,
            MarkerFill = OxyColors.MidnightBlue,
            TrackerFormatString = "{Tag}\n{1} : {2:mm\\:ss}\n{3} : {4:0.###}",
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

        public void InitializeSeries(IEnumerable<DataPoint> points)
        {
            ClearSeries(false);

            foreach (var p in  points)
            {
                Series.Points.Add(new OxyPlot.DataPoint(p.X, p.Y));
                Series.Points2.Add(new OxyPlot.DataPoint(p.X, p.Y));
            }

            // 最高点を求める
            var maxPoint = GetHeightestPoint(points);
            ScatterSeries.Points.Add(new ScatterPoint(maxPoint.X, maxPoint.Y, tag:"ピークの頂点"));

            // 開始点

            // 終了点

            // TODO ピークとして認められるかを確認

            // NG のときは、解除する

            PlotModel.InvalidatePlot(true);
        }

        /// <summary>
        /// 指定した列挙子から、最も高い <see cref="DataPoint.Y"/> を持つデータを取得します。
        /// </summary>
        /// <param name="points">列挙子。</param>
        /// <returns>最も高い信号強度を持つデータ。</returns>
        public DataPoint GetHeightestPoint(IEnumerable<DataPoint> points)
        {
            if (points == null) throw new NullReferenceException("指定した列挙子は null です。");
            if (points.Count() <= 0) throw new ArgumentOutOfRangeException("指定した列挙子は、適切なデータの数を持ちません。");

            DataPoint selectedPoint = null;
            var value = points.Max(p => p.Y);
            var maxPoints = points.Where(p => p.Y == value);
            var pointCount = (maxPoints?.Count() ?? 0);

            if (pointCount > 0)
            {
                // 偶数の最大値を持つときは -1 してインデックスを取得する
                var selectingIndex = (pointCount % 2 == 0) ? ((pointCount - 1) / 2) : (pointCount / 2);
                selectedPoint = maxPoints.ElementAt(selectingIndex);
            }
            else
            {
                throw new NullReferenceException("指定した列挙子の最大値の取得に失敗しました。");
            }

            return selectedPoint;
        }

        public void ClearSeries(bool isInvalidated = true)
        {
            Series.Points.Clear();
            Series.Points2.Clear();
            ScatterSeries.Points.Clear();

            if (isInvalidated)
            {
                PlotModel.InvalidatePlot(true);
            }
        }

        public override void SaveCsvFile(string filePath)
        {

        }

        #endregion
    }
}
