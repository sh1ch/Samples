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

namespace DataGridCustomCheckBoxTest
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<SampleData> Samples { get; } = new ObservableCollection<SampleData>
        {
            new SampleData { No = 1, Text = "エルルゥ", Data1 = true, Data2 = false },
            new SampleData { No = 2, Text = "アルルゥ", Data1 = false, Data2 = true },
            new SampleData { No = 3, Text = "ムックル", Data1 = false, Data2 = false },
        };


        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
        }
    }
}
