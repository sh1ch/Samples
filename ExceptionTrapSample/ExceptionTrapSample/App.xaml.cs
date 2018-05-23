using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ExceptionTrapSample
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            AppDomain.CurrentDomain.FirstChanceException += (sender, args) =>
            {
                ReportException("FirstChanceException.log", sender, args.Exception);
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                MessageBox.Show("未処理の例外が発生しました。アプリケーションを強制終了します。");
                Current.Shutdown();
            };
        }

        public App()
        {
            // UI スレッドの処理されない例外
            DispatcherUnhandledException += (sender, args) =>
            {
                var name = args.Exception.TargetSite.Name;
                var message = args.Exception.Message;

                var result = MessageBox.Show($"例外が {name} で発生しました。プログラムを継続しますか。{Environment.NewLine}詳細:{message}", "DispatcherUnhandledException", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    args.Handled = true;
                }
            };

        }

        /// <summary>
        /// 例外が発生したとき、ログファイルに例外情報を記録します。
        /// </summary>
        /// <param name="fileName">ログファイルの名前。</param>
        /// <param name="sender">例外の発生対象。</param>
        /// <param name="exception">発生した例外。</param>
        private static void ReportException(string fileName, object sender, Exception exception)
        {
            const string reportFormat = "===========================================================\r\n" +
                                        "ERROR Date = {0}, Sender = {1}, \r\n" +
                                        "{2}\r\n\r\n";

            var reportText = string.Format(reportFormat, DateTimeOffset.Now, sender, exception);
            File.AppendAllText(fileName, reportText);
        }

    }
}
