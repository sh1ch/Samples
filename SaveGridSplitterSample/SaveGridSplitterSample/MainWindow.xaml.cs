using System;
using System.Collections.Generic;
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

using settings = SaveGridSplitterSample.Properties.Settings;

namespace SaveGridSplitterSample
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Save();
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            Restore();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Restore();
        }

        private void Save()
        {
            var heights = SplitGrid.RowDefinitions.Select(p => p.ActualHeight).ToArray();

            settings.Default.GridRowHeight1 = heights[0];
            settings.Default.GridRowHeight3 = heights[2];
            settings.Default.Save();
        }

        private void Restore()
        {
            var height1 = settings.Default.GridRowHeight1;
            var height3 = settings.Default.GridRowHeight3;

            if (height1 > 0)
            {
                SplitGrid.RowDefinitions[0].Height = new GridLength(height1, GridUnitType.Star);
            }
            if (height3 > 0)
            {
                SplitGrid.RowDefinitions[2].Height = new GridLength(height3, GridUnitType.Star);
            }
        }

    }
}
