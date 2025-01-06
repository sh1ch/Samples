using BigCalendar.ViewModels;
using System.Configuration;
using System.Data;
using System.Windows;

namespace BigCalendar
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			var vm = new MainWindowViewModel();
			var v = new MainWindow
			{
				DataContext = vm
			};

			v.Show();
		}
	}

}
