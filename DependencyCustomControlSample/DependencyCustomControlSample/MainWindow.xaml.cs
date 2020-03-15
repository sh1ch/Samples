using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace DependencyCustomControlSample
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<SampleData> Records { get; } = new ObservableCollection<SampleData>();

        public MainWindow()
        {
            InitializeComponent();

            Records.Add(new SampleData { No = 1, Text = "ABCDEF", SubText = "ポイント", IsEnabled = false });
            Records.Add(new SampleData { No = 2, Text = "67890", SubText = "枚", IsEnabled = true });
            Records.Add(new SampleData { No = 3, Text = "100", SubText = "G", IsEnabled = false });
            Records.Add(new SampleData { No = 4, Text = "2000", SubText = "G", IsEnabled = true });

            DataContext = this;
        }
    }
}
