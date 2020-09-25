using CefSharp;
using CefSharp.Wpf;
using System.Windows;

namespace ChromiumTest
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            InitializeCef();

            var window = new MainWindow();

            window.Show();
        }

        private void InitializeCef()
        {
            var settings = new CefSettings();

            settings.CachePath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "cache");
            settings.UserDataPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "userdata");

            settings.Locale = "ja";
            settings.AcceptLanguageList = "ja-JP";

            Cef.Initialize(settings);
        }
    }
}
