using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace ExceptionTrapSample
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

        private void Button_Click_1A(object sender, RoutedEventArgs e)
        {
            // UI スレッドで例外を発生させる
            throw new InvalidOperationException($"ThreadID:{Thread.CurrentThread.ManagedThreadId}");
        }

        private void Button_Click_2A(object sender, RoutedEventArgs e)
        {
            int a = 0;
            int b = 0;

            var error = a / b; // 0 の除算
        }

        private void Button_Click_3A(object sender, RoutedEventArgs e)
        {
            var task = Task.Run(() => 
            {
                // UI スレッドで例外を発生させる
                throw new InvalidOperationException($"ThreadID:{Thread.CurrentThread.ManagedThreadId}");
            });

            // 待たないとエラーを捕捉できない
            task.Wait();
        }

        private async void Button_Click_4A(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => 
            {
                throw new InvalidOperationException($"ThreadID:{Thread.CurrentThread.ManagedThreadId}");
            });
        }

        private void Button_Click_1B(object sender, RoutedEventArgs e)
        {
            try
            {
                // UI スレッドで例外を発生させる
                throw new InvalidOperationException($"ThreadID:{Thread.CurrentThread.ManagedThreadId}");
            }
            catch
            {
                MessageBox.Show($"ThreadID:{Thread.CurrentThread.ManagedThreadId} 例外が発生しました。", "try-catch ステートメントより");
            }
        }

        private void Button_Click_2B(object sender, RoutedEventArgs e)
        {
            try
            {
                int a = 0;
                int b = 0;

                var error = a / b; // 0 の除算
            }
            catch
            {
                MessageBox.Show($"ThreadID:{Thread.CurrentThread.ManagedThreadId} 例外が発生しました。", "try-catch ステートメントより");
            }
        }

        private void Button_Click_3B(object sender, RoutedEventArgs e)
        {
            var task = Task.Run(() =>
            {
                // UI スレッドで例外を発生させる
                throw new InvalidOperationException($"ThreadID:{Thread.CurrentThread.ManagedThreadId}");
            });

            try
            {
                task.Wait();
            }
            catch
            {
                MessageBox.Show($"ThreadID:{Thread.CurrentThread.ManagedThreadId} 例外が発生しました。", "try-catch ステートメントより");
            }

        }

        private async void Button_Click_4B(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                try
                {
                    // UI スレッドで例外を発生させる
                    throw new InvalidOperationException($"ThreadID:{Thread.CurrentThread.ManagedThreadId}");
                }
                catch
                {
                    MessageBox.Show($"ThreadID:{Thread.CurrentThread.ManagedThreadId} 例外が発生しました。", "try-catch ステートメントより");
                }
            });
        }
    }
}
