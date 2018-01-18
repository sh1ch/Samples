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

using prop = SaveWindowPlacementStateSample.Properties;

namespace SaveWindowPlacementStateSample
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private WindowPlacementStateSetting _Setting = new WindowPlacementStateSetting();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            if (_Setting.Placement != null)
            {
                _Setting.Attach(this);
            }
            else
            {
                MessageBox.Show("ウィンドウの保存された情報がありません。");
            }
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            _Setting.Read(this);
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            _Setting.Placement = prop.Settings.Default.WindowPlacement;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _Setting.Read(this);

            // ウィンドウの位置情報を Settings.settings に保存
            prop.Settings.Default.WindowPlacement = _Setting.Placement;
            prop.Settings.Default.Save();
        }
    }
}
