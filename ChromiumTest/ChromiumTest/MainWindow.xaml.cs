using CefSharp;
using CefSharp.Wpf;
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

namespace ChromiumTest
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private CefSharp.Wpf.ChromiumWebBrowser _Browser = null;

        public MainWindow()
        {
            // XAML を初期化する前に Cef を初期化すること
            InitializeComponent();

        }

        public void InitializeChromium()
        {
            
        }

        private void ChromiumWebBrowser_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _Browser = sender as CefSharp.Wpf.ChromiumWebBrowser;

            if (_Browser == null) return;
            if (!_Browser.IsBrowserInitialized) return;

            var htmlFile = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"sample.html");
            var html = new StreamReader(htmlFile, Encoding.UTF8)?.ReadToEnd();

            _Browser.LoadHtml(html, System.AppDomain.CurrentDomain.BaseDirectory);
            /*
            _Browser.FrameLoadStart += (obj, args) => 
            {
                var browser = obj as CefSharp.Wpf.ChromiumWebBrowser;
                
                if (browser == null) return;

                if (args.Frame.IsMain)
                {
                    var zoomlevel = (50F - 100F) / 25F;

                    browser.SetZoomLevel(zoomlevel);
                }
            };
            */
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _Browser.Print();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var settings = new PdfPrintSettings();

            settings.Landscape = true;

            _Browser.PrintToPdfAsync("aaa.pdf", settings);
        }

        private void Button_Click_ZoomIn(object sender, RoutedEventArgs e)
        {
            _Browser.ZoomInCommand.Execute(null);
        }

        private void Button_Click_ZoomOut(object sender, RoutedEventArgs e)
        {
            _Browser.ZoomOutCommand.Execute(null);
        }

        private void Button_Click_ZoomReset(object sender, RoutedEventArgs e)
        {
            _Browser.ZoomResetCommand.Execute(null);
        }

        private void Grid_Initialized(object sender, EventArgs e)
        {
            var grid = sender as System.Windows.Controls.Grid;

            if (grid == null) return;

            _Browser = new ChromiumWebBrowser();

            _Browser.ZoomLevelIncrement = 0.10F;
            _Browser.IsBrowserInitializedChanged += ChromiumWebBrowser_IsBrowserInitializedChanged;

            grid.Children.Add(_Browser);
        }
    }
}
