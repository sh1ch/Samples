using OxyPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OxyPlotSample
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public OxyPlotView_Model Sample1 { get; } = new OxyPlotView_Model();
        public OxyPlot_Model Sample2 { get; } = new OxyPlot_Model();

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            Sample1.Init();
            Sample2.Init(plot, x, y, series);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Sample1.AddItem();
            Sample2.AddItem();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Sample1.InitSeries();
            Sample2.InitSeries();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Sample1.InitAxisX();
            Sample1.InitAxisY();

            Sample2.InitAxisX();
            Sample2.InitAxisY();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Sample1.InitController();
            Sample2.InitController();
        }

        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            Sample1.SaveImage();
            Sample2.SaveImage();
        }

    }
}
