using HeritageFramework;
using HeritageFramework.Mathematics.NumericalAnalysis;
using HeritageFramework.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreaCalculator.Models
{
    /// <summary>
    /// <see cref="PeakResult"/> クラスは、ピークの分析結果を表現するクラスです。
    /// </summary>
    public class PeakResult : INotifyPropertyChanged
    {
        #region Properties

        private DataPoint _TopPoint;

        /// <summary>
        /// ピークの頂点を表すポイントを取得または設定します。
        /// </summary>
        public DataPoint TopPoint
        {
            get { return _TopPoint; }
            set
            {
                _TopPoint = value;
                PropertyChanged.Raise(this, nameof(TopPoint));
            }
        }

        private DataPoint _StartPoint;

        /// <summary>
        /// ピークの開始位置を表すポイントを取得または設定します。
        /// </summary>
        public DataPoint StartPoint
        {
            get { return _StartPoint; }
            set
            {
                _StartPoint = value;
                PropertyChanged.Raise(this, nameof(StartPoint));
                PropertyChanged.Raise(this, nameof(BaseLine));
            }
        }

        private DataPoint _EndPoint;

        /// <summary>
        /// ピークの終了位置を表すポイントを取得または設定します。
        /// </summary>
        public DataPoint EndPoint
        {
            get { return _EndPoint; }
            set
            {
                _EndPoint = value;
                PropertyChanged.Raise(this, nameof(EndPoint));
                PropertyChanged.Raise(this, nameof(BaseLine));
            }
        }

        public Tuple<double, double> BaseLine
        {
            get
            {
                if (IsEnabled == false) return null;
                
                return GetBaseLine(StartPoint, EndPoint);
            }
        }
            

        public bool IsEnabled
        {
            get
            {
                // ピークの頂点が揃わないとき、ピークとして認めない
                if (TopPoint == null || StartPoint == null || EndPoint == null)
                {
                    return false;
                }

                return true;
            }
        }

        private double _Area;

        /// <summary>
        /// ピークの面積値を取得します。
        /// </summary>
        public double Area
        {
            get { return _Area; }
            private set
            {
                _Area = value;
                PropertyChanged.Raise(this, nameof(Area));
            }
        }

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="PeakResult"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public PeakResult()
        {

        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Methods

        public void UpdatePeakArea(IEnumerable<DataPoint> points)
        {
            if (IsEnabled == false)
            {
                Area = double.NaN;
                return;
            }

            var startSeconds = StartPoint.X;
            var endSeconds = EndPoint.X;
            var peakPoints = points.Where(p => p.X >= startSeconds && p.X <= endSeconds);

            var x = BaseLine.Item1;
            var y = BaseLine.Item2;

            double r = 0.0D;
            DataPoint p1 = null; // peak top left
            DataPoint p2 = null; // peak top right
            DataPoint p3 = null; // peak bottom left
            DataPoint p4 = null; // peak bottom right

            using (var writer = new StreamWriter("latest_calc_peak_area.txt", false, Encoding.UTF8))
            {
                // ピーク中の点
                if ((peakPoints?.Count() ?? 0) > 0)
                {
                    foreach (var p in peakPoints)
                    {
                        p1 = p2;
                        p2 = p;
                        p3 = p4;

                        var px = p.X;
                        var py = p.X * x + y;

                        // 実際の位置がベースラインよりも低いときは、実際の点の位置を優先する
                        p4 = (p.Y > py) ? new DataPoint(p.X, p.X * x + y) : new DataPoint(p.X, p.Y);

                        if (p1 != null && p2 != null)
                        {
                            var h = ((p1.Y - p3.Y) * 1000 + (p2.Y - p4.Y) * 1000);
                            var area = (h) * (p4.X - p3.X) / 2.0D;
                            r += area;

                            writer.WriteLine($"{p1.X}, {area}, (({p1.Y} - {p3.Y}) + ({p2.Y} - {p4.Y})) * ({p4.X} - {p3.X}) / 2.0D");
                        }
                    }

                    Area = r;
                }
            }
        }

        public void ForceSetPeakPoint(IEnumerable<DataPoint> points, double startSeconds, double endSeconds)
        {
            var startPoint = points.SingleOrDefault(p => p.X == startSeconds);
            var endPoint = points.SingleOrDefault(p => p.X == endSeconds);

            if (startPoint != null && endPoint != null)
            {
                StartPoint = startPoint;
                EndPoint = endPoint;
            }
        }

        public void MeasurePeakPoints(IEnumerable<DataPoint> points, double startSlope, double endSlope, double minSeconds)
        {
            if (points == null) throw new NullReferenceException("指定した列挙子は null です。");
            if (points.Count() <= 0) throw new ArgumentOutOfRangeException("指定した列挙子は、適切なデータの数を持ちません。");

            // 最高点を求める
            var topPoint = GetHeightestPoint(points);
            if (topPoint == null) return;

            DataPoint startPoint = null;
            DataPoint endPoint = null;

            var startSeconds = topPoint.X - (minSeconds / 2.0D);

            // 開始点 (0.1秒ずつカウントダウンして走査)
            for (var i = startSeconds; i >= 0; i -= 0.1)
            {
                var factorPoints = points.Skip((Convert.ToInt32(i * 10))).Take(5 * 10);
                if ((factorPoints?.Count() ?? 0) != 50) break;

                var slope = factorPoints.Average(p => p.NextSlope) * 1000;

                if (Math.Abs(slope) < Math.Abs(startSlope))
                {
                    startPoint = factorPoints.ElementAt(0);
                    break;
                }
            }

            var endSeconds = points.Max(p => p.X) - (5 * 10); // 5 秒手前まで

            // 終了点 (0.1秒ずつカウントアップして走査) (開始時点 i は + 最小幅(s) / 2)
            for (var i = topPoint.X + (minSeconds / 2.0D); i <= endSeconds; i += 0.1)
            {
                var factorPoints = points.Skip((Convert.ToInt32(i * 10))).Take(5 * 10);
                if ((factorPoints?.Count() ?? 0) != 50) break;

                var slope = factorPoints.Average(p => p.NextSlope) * 1000;

                if (Math.Abs(slope) < Math.Abs(endSlope))
                {
                    endPoint = factorPoints.ElementAt(0);
                    break;
                }
            }

            TopPoint = topPoint;
            StartPoint = startPoint;
            EndPoint = endPoint;
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

        public Tuple<double, double> GetBaseLine(DataPoint p1, DataPoint p2)
        {
            if (p1 == null || p2 == null) throw new NullReferenceException("指定したデータは null です。");

            var matrix = new GaussMatrix(2);

            matrix.A = new double[,]
            {
                { p2.X, 1 },
                { p1.X, 1 }
            };
            matrix.B = new double[] { p2.Y, p1.Y };

            // ピポッド評価
            if (matrix.SetPivot() == false)
            {
                return Tuple.Create(double.NaN, double.NaN);
            }

            matrix.Calculate();

            return Tuple.Create(matrix.B[0], matrix.B[1]); // 傾き m

        }

        #endregion
    }
}
