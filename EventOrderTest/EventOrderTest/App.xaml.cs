using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EventOrderTest;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private static readonly NLog.Logger _Logger = NLog.LogManager.GetCurrentClassLogger();

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _Logger.Info("OnStartup called.");

        var vm = new MainWindowViewModel();
        var v = new MainWindow
        {
            DataContext = vm,
        };

        v.ShowDialog();
    }
}
