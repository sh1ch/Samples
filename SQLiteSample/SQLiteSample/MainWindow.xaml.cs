using System;
using System.Collections.Generic;
using System.Linq;
using SQLiteLib.Orm;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
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

namespace SQLiteSample
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var connection = @"version=3;data source=Sample.db;cache size=10000;default timeout=5000;busytimeout=10000;foreign keys=True";

            using (var context = new MasterContext(new SQLiteConnection(connection)))
            {
                var users = context.Users.ToList();

                foreach (var user in users)
                {
                    Label.Content += $"{user.Name} の技は {user.Data2} です。\r\n";
                }
            }
        }
    }
}
