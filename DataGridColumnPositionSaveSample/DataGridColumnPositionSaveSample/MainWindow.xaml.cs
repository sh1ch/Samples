using System;
using System.Collections.Generic;
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

namespace DataGridColumnPositionSaveSample
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _File = "datagrid.settings.json";
        private MainWindowViewModel _VM = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = _VM;
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            if (!File.Exists(_File)) return;

            DataGridColumnSettings.Load(_File, DataGrid.Columns);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DataGridColumnSettings.Save(_File, DataGrid.Columns);
        }
    }
}
