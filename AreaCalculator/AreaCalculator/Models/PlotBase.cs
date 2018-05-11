using HeritageFramework.Wpf;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AreaCalculator.Models
{
    /// <summary>
    /// <see cref="PlotBase"/> クラスは、シグナルモニタープロットの基底クラスです。
    /// </summary>
    public abstract class PlotBase
    {
        #region Properties

        private PlotModel _PlotModel = new PlotModel();

        /// <summary>
        /// プロットを取得します。
        /// </summary>
        public PlotModel PlotModel
        {
            get { return _PlotModel; }
            private set
            {
                _PlotModel = value;
            }
        }

        private PlotController _GestureController = new PlotController();

        /// <summary>
        /// グラフをマウス操作するコントローラーを取得します。
        /// </summary>
        public PlotController GestureController
        {
            get { return _GestureController; }
            private set
            {
                _GestureController = value;
            }
        }

        private TimeSpanAxis _AxisX = new TimeSpanAxis();

        /// <summary>
        /// 時間の経過を表す X 軸を取得します。
        /// </summary>
        public TimeSpanAxis AxisX
        {
            get { return _AxisX; }
            set
            {
                _AxisX = value;
            }
        }

        private LinearAxis _AxisY = new LinearAxis();

        /// <summary>
        /// 大きさを表す Y 軸を取得します。
        /// </summary>
        public LinearAxis AxisY
        {
            get { return _AxisY; }
            set
            {
                _AxisY = value;
            }
        }

        #endregion

        #region Initializes

        /// <summary>
        /// <see cref="PlotBase"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public PlotBase()
        {
            GestureController.InitializeBind();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// <see cref="PlotModel"/> モデルを更新します。
        /// </summary>
        public void Update()
        {
            PlotModel.InvalidatePlot(true);
        }

        /// <summary>
        /// プロットを初期化します。
        /// </summary>
        /// <param name="title">プロットのタイトル。</param>
        public void InitializePlot(string title)
        {
            PlotModel.Title = title;
        }

        /// <summary>
        /// グラフにエリアを追加します。
        /// </summary>
        /// <param name="title">エリアのタイトル。</param>
        /// <param name="color">エリアの色。</param>
        /// <param name="minX">横軸の最小値。</param>
        /// <param name="maxX">横軸の最大値。</param>
        /// <param name="minY">縦軸の最小値。</param>
        /// <param name="maxY">縦軸の最大値。</param>
        public void AddArea(string title, OxyColor color, int minX, int maxX, int minY, int maxY)
        {
            var area = new RectangleAnnotation
            {
                Fill = color,
                MinimumX = minX,
                MaximumX = maxX,
                MinimumY = minY,
                MaximumY = maxY,
                Text = title,
                TextVerticalAlignment = OxyPlot.VerticalAlignment.Bottom,
                Layer = AnnotationLayer.BelowAxes,
            };

            var line = new LineAnnotation
            {
                Color = OxyColors.Black,
                Type = LineAnnotationType.Vertical,
                X = maxX,
            };

            PlotModel.Annotations.Add(area);
            PlotModel.Annotations.Add(line);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// X 軸を初期化します。
        /// </summary>
        /// <param name="min">表示の最小値。</param>
        /// <param name="absMin">可変できる表示の最小値。</param>
        /// <param name="minRange">最小の表示幅。</param>
        protected void InitializeAxisX(double min, double absMin, double minRange)
        {
            AxisX.Position = AxisPosition.Bottom;
            AxisX.MajorGridlineColor = OxyColor.FromArgb(0xCC, 0xE3, 0xE3, 0xE3);
            AxisX.MajorGridlineStyle = LineStyle.Solid;
            AxisX.MajorGridlineThickness = 1;
            AxisX.MinorGridlineColor = OxyColor.FromArgb(0x99, 0xE3, 0xE3, 0xE3);
            AxisX.MinorGridlineStyle = LineStyle.Dot;
            AxisX.MinorGridlineThickness = 1;
            // AxisX.StringFormat = "mm:ss";

            // カスタム要素
            AxisX.Minimum = min;
            AxisX.AbsoluteMinimum = absMin;
            // AxisX.Maximum = max;
            // AxisX.AbsoluteMaximum = absMax;
            AxisX.MinimumRange = minRange;
        }

        /// <summary>
        /// X 軸を初期化します。
        /// </summary>
        /// <param name="min">表示の最小値。</param>
        /// <param name="absMin">可変できる表示の最小値。</param>
        /// <param name="max">表示の最大値。</param>
        /// <param name="absMax">可変できる表示の最大値。</param>
        /// <param name="minRange">最小の表示幅。</param>
        protected void InitializeAxisX(double min, double absMin, double max, double absMax, double minRange)
        {
            AxisX.Position = AxisPosition.Bottom;
            AxisX.MajorGridlineColor = OxyColor.FromArgb(0xCC, 0xE3, 0xE3, 0xE3);
            AxisX.MajorGridlineStyle = LineStyle.Solid;
            AxisX.MajorGridlineThickness = 1;
            AxisX.MinorGridlineColor = OxyColor.FromArgb(0x99, 0xE3, 0xE3, 0xE3);
            AxisX.MinorGridlineStyle = LineStyle.Dot;
            AxisX.MinorGridlineThickness = 1;
            AxisX.StringFormat = "mm:ss";

            // カスタム要素
            AxisX.Minimum = min;
            AxisX.AbsoluteMinimum = absMin;
            AxisX.Maximum = max;
            AxisX.AbsoluteMaximum = absMax;
            AxisX.MinimumRange = minRange;
        }

        /// <summary>
        /// 指定した秒数で X 軸を初期化します。
        /// </summary>
        /// <param name="minSeconds">最小秒数。</param>
        /// <param name="seconds">合計秒数。</param>
        public void InitializeAxisX(int minSeconds, int maxSeconds)
        {
            AxisX.Minimum = minSeconds;
            AxisX.AbsoluteMinimum = minSeconds;

            AxisX.Maximum = maxSeconds;
            AxisX.AbsoluteMaximum = maxSeconds;

            if (AxisX.MinimumRange >= AxisX.AbsoluteMaximum)
            {
                AxisX.Maximum = AxisX.MinimumRange;
                AxisX.AbsoluteMaximum = AxisX.MinimumRange;
            }
        }

        /// <summary>
        /// Y 軸を初期化します。
        /// </summary>
        /// <param name="min">表示の最小値。</param>
        /// <param name="absMin">可変できる表示の最小値。</param>
        /// <param name="max">表示の最大値。</param>
        /// <param name="absMax">可変できる表示の最大値。</param>
        /// <param name="minRange">最小の表示幅。</param>
        protected void InitializeAxisY(double min, double absMin, double max, double absMax, double minRange)
        {
            AxisY.Position = AxisPosition.Left;
            AxisY.MajorGridlineColor = OxyColor.FromArgb(0xCC, 0xE3, 0xE3, 0xE3);
            AxisY.MajorGridlineStyle = LineStyle.Solid;
            AxisY.MajorGridlineThickness = 1;
            AxisY.MinorGridlineColor = OxyColor.FromArgb(0x99, 0xE3, 0xE3, 0xE3);
            AxisY.MinorGridlineStyle = LineStyle.Dot;
            AxisY.MinorGridlineThickness = 1;

            // カスタム要素
            AxisY.Minimum = min;
            AxisY.AbsoluteMinimum = absMin;
            AxisY.Maximum = max;
            AxisY.AbsoluteMaximum = absMax;
            AxisY.MinimumRange = minRange;
        }

        /// <summary>
        /// プロットの画像を保存します。
        /// </summary>
        /// <param name="filePath">保存するファイルパス。</param>
        public virtual void SavePngFile(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                OxyPlot.Wpf.PngExporter.Export(PlotModel, stream, 800, 600, OxyColor.FromArgb(0xff, 0xff, 0xff, 0xff));
            }
        }

        public abstract void SaveCsvFile(string filePath);

        /// <summary>
        /// CSV ファイルを保存します。
        /// </summary>
        /// <param name="filePath">保存するファイルパス。</param>
        /// <param name="seconds">保存するデータ秒数 (件数)。</param>
        /// <param name="data">保存するデータ。</param>
        public virtual void SaveCsvFile(string filePath, IEnumerable<int> seconds, List<Tuple<string, IEnumerable<string>>> data)
        {
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (var writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    // タイトル
                    writer.WriteLine(PlotModel.Title);

                    // 列定義
                    var titles = data.Select(p => p.Item1);
                    writer.WriteLine("," + string.Join(",", titles)); // 時間, data1, data2, data3...

                    var times = seconds.OrderBy(p => p).Select(p => TimeSpan.FromSeconds(p).ToString("mm':'ss")).ToArray();
                    var items = data.Select(p => p.Item2).Select(p => p.ToArray());

                    for (var i = 0; i < times.Count(); i++)
                    {
                        var time = times[i];
                        var item = items.Select(p => p[i]).ToArray();

                        writer.WriteLine(string.Join(",", time, string.Join(",", item)));
                    }
                }
            }
        }

        #endregion

    }
}
