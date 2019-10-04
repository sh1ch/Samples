using Microsoft.WindowsAPICodePack.Dialogs;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileOpenDialogSample
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

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            var browser = new CommonOpenFileDialog();

            browser.Title = "フォルダーを選択してください（１）";
            browser.IsFolderPicker = true;

            if (browser.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Path1Lable.Content = browser.FileName;
            }
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            var browser = new System.Windows.Forms.FolderBrowserDialog();

            browser.Description = "フォルダーを選択してください（２）";

            if (browser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Path2Lable.Content = browser.SelectedPath;
            }
        }

        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            var result = Dialogs.DialogResult.None;
            var browser = new Dialogs.FolderBrowserDialog();
            
            browser.Title = "フォルダーを選択してください（３）";
            browser.SelectedPath = Path3Lable.Content.ToString();

            // ウィンドウが取得できるときは設定する
            var obj = sender as DependencyObject;

            if (obj != null)
            {
                var window = Window.GetWindow(obj);

                if (window != null) result = browser.ShowDialog(window);
            }
            else
            {
                result = browser.ShowDialog(IntPtr.Zero);
            }

            if (result == Dialogs.DialogResult.OK)
            {
                Path3Lable.Content = browser.SelectedPath;
            }
        }
    }
}
