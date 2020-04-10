using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PrinterIconSample
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public IEnumerable<Tuple<string, BitmapSource>> InstalledPrinters { get; set; }


        public MainWindow()
        {
            InitializeComponent();

            Console.WriteLine($"Total Memory = {GC.GetTotalMemory(true) / 1024} KB");

            // for (var i = 0; i < 100; i++)
            {
                InstalledPrinters = PrinterInfo.GetInstalledPrinters();
            }

            Console.WriteLine($"Total Memory = {GC.GetTotalMemory(true) / 1024} KB");

            DataContext = this;
        }

        
    }

    
}
