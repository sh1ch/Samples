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

namespace OxyPlotSample
{
    public class OxyPlotView_Model
    {
        #region Properties

        public PlotModel Model { get; } = new PlotModel();
        public PlotController Controller { get; } = new PlotController();

        public OxyPlot.Axes.TimeSpanAxis X { get; } = new OxyPlot.Axes.TimeSpanAxis();
        public OxyPlot.Axes.LinearAxis Y { get; } = new OxyPlot.Axes.LinearAxis();
        public OxyPlot.Series.LineSeries LineSeries { get; private set; }
        public OxyPlot.Series.FunctionSeries FunctionSeries { get; private set; }

        public ObservableCollection<TestData> Samples { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// <see cref="OxyPlotView_Model"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public OxyPlotView_Model()
        {

        }

        #endregion

        #region Public Methods

        public void Init()
        {
            Samples = new ObservableCollection<TestData>
            {
                new TestData{ Time= new TimeSpan(0,0,0), Value=0, Tag="A" },
                new TestData{ Time= new TimeSpan(0,0,1), Value=2, Tag="B" },
                new TestData{ Time= new TimeSpan(0,0,2), Value=4, Tag="C" },
                new TestData{ Time= new TimeSpan(0,0,3), Value=6, Tag="D" },
                new TestData{ Time= new TimeSpan(0,0,4), Value=0, Tag="E" },
                new TestData{ Time= new TimeSpan(0,0,5), Value=2, Tag="F" },
            };

            Model.Title = "PlotView";

            // 軸の初期化
            X.Position = OxyPlot.Axes.AxisPosition.Bottom;
            Y.Position = OxyPlot.Axes.AxisPosition.Left;

            // 線グラフ
            LineSeries = new OxyPlot.Series.LineSeries();
            LineSeries.Title = "Custom";
            LineSeries.ItemsSource = Samples;
            LineSeries.DataFieldX = nameof(TestData.Time);
            LineSeries.DataFieldY = nameof(TestData.Value);

            var a = 1;
            var b = 2;

            // 関数グラフ
            FunctionSeries = new OxyPlot.Series.FunctionSeries
            (
                x => a * x + b, 0, 30, 5, "Y = ax + b"
            );

            Model.Axes.Add(X);
            Model.Axes.Add(Y);
            Model.Series.Add(LineSeries);
            Model.Series.Add(FunctionSeries);

            Model.InvalidatePlot(true);
        }

        public void InitController()
        {
            // グラフのマウス操作およびキー操作の初期化
            Controller.UnbindKeyDown(OxyKey.A);
            Controller.UnbindKeyDown(OxyKey.C, OxyModifierKeys.Control);
            Controller.UnbindKeyDown(OxyKey.C, OxyModifierKeys.Control | OxyModifierKeys.Alt);
            Controller.UnbindKeyDown(OxyKey.R, OxyModifierKeys.Control | OxyModifierKeys.Alt);
            Controller.UnbindTouchDown();

            Controller.UnbindMouseDown(OxyMouseButton.Left);
            Controller.UnbindMouseDown(OxyMouseButton.Middle);
            Controller.UnbindMouseDown(OxyMouseButton.Right);

            Controller.BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt);
            Controller.BindMouseDown(OxyMouseButton.Middle, PlotCommands.PointsOnlyTrack);
            Controller.BindMouseDown(OxyMouseButton.Right, PlotCommands.ZoomRectangle);
        }

        public void InitSeries()
        {
            LineSeries.CanTrackerInterpolatePoints = true;
            LineSeries.Smooth = false;
            LineSeries.StrokeThickness = 5;
            LineSeries.Color = OxyColors.Red;
            LineSeries.TrackerFormatString = "{Tag}\n{1} : {2:mm\\:ss\\.f}\n{3} : {4:0.000}";

            Model.InvalidatePlot(true);
        }

        public void InitAxisX()
        {
            X.Position = AxisPosition.Bottom;
            X.MajorGridlineColor = OxyColor.FromArgb(0xCC, 0xE3, 0xE3, 0xE3);
            X.MajorGridlineStyle = LineStyle.Solid;
            X.MajorGridlineThickness = 1;
            X.MinorGridlineColor = OxyColor.FromArgb(0x99, 0xE3, 0xE3, 0xE3);
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

            Model.InvalidatePlot(true);
        }

        public void InitAxisY()
        {
            Y.Position = AxisPosition.Left;
            Y.MajorGridlineColor = OxyColor.FromArgb(0xCC, 0xE3, 0xE3, 0xE3);
            Y.MajorGridlineStyle = LineStyle.Solid;
            Y.MajorGridlineThickness = 1;
            Y.MinorGridlineColor = OxyColor.FromArgb(0x99, 0xE3, 0xE3, 0xE3);
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

            Model.InvalidatePlot(true);
        }

        public void AddItem()
        {
            var i = Samples.Count();
            Samples.Add(new TestData { Time = new TimeSpan(0, 0, i), Value = i, Tag = $"ADD:{i}" });

            Model.InvalidatePlot(true);
        }

        public void SaveImage()
        {
            using (var stream = new FileStream("output_1.png", FileMode.Create))
            {
                var width = 800;
                var height = 600;

                OxyPlot.Wpf.PngExporter.Export(Model, stream, width, height, OxyColor.FromArgb(0xff, 0xff, 0xff, 0xff));
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
