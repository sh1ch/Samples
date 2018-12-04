using OxyPlot;
using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace OxyPlotSample
{
    public class OxyPlot_Model
    {
        #region Properties

        public OxyPlot.Wpf.Plot Plot { get; private set; }

        public OxyPlot.Wpf.TimeSpanAxis X { get; private set; }
        public OxyPlot.Wpf.LinearAxis Y { get; private set; }
        public OxyPlot.Wpf.LineSeries LineSeries { get; private set; }

        public ObservableCollection<TestData> Samples { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// <see cref="OxyPlot_Model"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public OxyPlot_Model()
        {

        }

        #endregion

        #region Public Methods

        public void Init(OxyPlot.Wpf.Plot plot, OxyPlot.Wpf.TimeSpanAxis x, OxyPlot.Wpf.LinearAxis y, OxyPlot.Wpf.LineSeries series)
        {
            Plot = plot;
            X = x;
            Y = y;
            LineSeries = series;

            Samples = new ObservableCollection<TestData>
            {
                new TestData{ Time= new TimeSpan(0,0,0), Value=0, Tag="A" },
                new TestData{ Time= new TimeSpan(0,0,1), Value=2, Tag="B" },
                new TestData{ Time= new TimeSpan(0,0,2), Value=4, Tag="C" },
                new TestData{ Time= new TimeSpan(0,0,3), Value=6, Tag="D" },
                new TestData{ Time= new TimeSpan(0,0,4), Value=0, Tag="E" },
                new TestData{ Time= new TimeSpan(0,0,5), Value=2, Tag="F" },
            };

            Plot.Title = "WPF Plot";
        }

        public void InitController()
        {
            // グラフのマウス操作およびキー操作の初期化
            var controller = Plot.ActualController;

            controller.UnbindKeyDown(OxyKey.A);
            controller.UnbindKeyDown(OxyKey.C, OxyModifierKeys.Control);
            controller.UnbindKeyDown(OxyKey.C, OxyModifierKeys.Control | OxyModifierKeys.Alt);
            controller.UnbindKeyDown(OxyKey.R, OxyModifierKeys.Control | OxyModifierKeys.Alt);
            controller.UnbindTouchDown();

            controller.UnbindMouseDown(OxyMouseButton.Left);
            controller.UnbindMouseDown(OxyMouseButton.Middle);
            controller.UnbindMouseDown(OxyMouseButton.Right);

            controller.BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt);
            controller.BindMouseDown(OxyMouseButton.Middle, PlotCommands.PointsOnlyTrack);
            controller.BindMouseDown(OxyMouseButton.Right, PlotCommands.ZoomRectangle);
        }

        public void InitSeries()
        {
            LineSeries.CanTrackerInterpolatePoints = true;
            LineSeries.Smooth = false;
            LineSeries.StrokeThickness = 5;
            LineSeries.Color = System.Windows.Media.Colors.Blue;
            LineSeries.TrackerFormatString = "{Tag}\n{1} : {2:mm\\:ss\\.f}\n{3} : {4:0.000}";
        }

        public void InitAxisX()
        {
            X.Position = AxisPosition.Bottom;
            X.MajorGridlineColor = System.Windows.Media.Color.FromArgb(0xCC, 0xE3, 0xE3, 0xE3);
            X.MajorGridlineStyle = LineStyle.Solid;
            X.MajorGridlineThickness = 1;
            X.MinorGridlineColor = System.Windows.Media.Color.FromArgb(0x99, 0xE3, 0xE3, 0xE3);
            X.MinorGridlineStyle = LineStyle.Dot;
            X.MinorGridlineThickness = 1;

            var min = 0;
            var absMin = -10;
            var max = 100;
            var absMax = 120;

            // 表示領域
            X.Minimum = min;
            X.AbsoluteMinimum = absMin;
            X.Maximum = max;
            X.AbsoluteMaximum = absMax;

            var minRange = 10;
            var maxRange = 200;

            // 表示サイズの最大最小
            X.MinimumRange = minRange;
            X.MaximumRange = maxRange;
        }

        public void InitAxisY()
        {
            Y.Position = AxisPosition.Left;
            Y.MajorGridlineColor = System.Windows.Media.Color.FromArgb(0xCC, 0xE3, 0xE3, 0xE3);
            Y.MajorGridlineStyle = LineStyle.Solid;
            Y.MajorGridlineThickness = 1;
            Y.MinorGridlineColor = System.Windows.Media.Color.FromArgb(0x99, 0xE3, 0xE3, 0xE3);
            Y.MinorGridlineStyle = LineStyle.Dot;
            Y.MinorGridlineThickness = 1;

            var min = 0;
            var absMin = -10;
            var max = 50;
            var absMax = 60;

            // 表示領域
            Y.Minimum = min;
            Y.AbsoluteMinimum = absMin;
            Y.Maximum = max;
            Y.AbsoluteMaximum = absMax;

            var minRange = 10;
            var maxRange = 70;

            // 表示サイズの最大最小
            Y.MinimumRange = minRange;
            Y.MaximumRange = maxRange;
        }

        public void AddItem()
        {
            var i = Samples.Count();
            Samples.Add(new TestData { Time = new TimeSpan(0, 0, i), Value = i, Tag = $"ADD:{i}" });
        }

        public void SaveImage()
        {
            var source = Plot.ToBitmap();
            // plot.SaveBitmap("output.png"); // BMP形式

            using (var stream = new FileStream("output_2.png", FileMode.Create))
            {
                var encoder = new PngBitmapEncoder(); // その他の形式

                encoder.Frames.Add(BitmapFrame.Create(source));
                encoder.Save(stream);
            }
        }

        #endregion

        public class TestData
        {
            public TimeSpan Time { get; set; }
            public double Value { get; set; }
            public string Tag { get; set; }
        }
    }
}
