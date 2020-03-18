using Codeplex.Data;
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
        private MainWindowViewModel _VM = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = _VM;
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            var text = "";

            if (!File.Exists("datagrid.json")) return;

            using (var reader = new StreamReader("datagrid.json", Encoding.UTF8))
            {
                text = reader.ReadToEnd();
            }

            if (string.IsNullOrEmpty(text)) return;

            var json = DynamicJson.Parse(text);
            
            foreach (DataGridParameter param in json)
            {
                var column = DataGrid.Columns.SingleOrDefault(p => p.Header.ToString() == param.Header);

                if (column != null)
                {
                    column.DisplayIndex = param.DisplayIndex;
                    column.Width = param.Width;
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var columns = DataGrid.Columns
                .Select(p => new DataGridParameter { Header = p.Header.ToString(), DisplayIndex = p.DisplayIndex, Width = p.ActualWidth });

            var text = DynamicJson.Serialize(columns);

            using (var writer = new StreamWriter("datagrid.json", false, Encoding.UTF8))
            {
                writer.Write(text);
            }
        }
    }
}
