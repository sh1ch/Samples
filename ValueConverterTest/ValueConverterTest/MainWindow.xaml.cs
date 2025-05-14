using CommunityToolkit.Mvvm.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ValueConverterTest;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
[ObservableObject]
public partial class MainWindow : Window
{
    [ObservableProperty]
    private string _Text1 = "A";
    [ObservableProperty]
    private string _Param = "Param3-2";


	public MainWindow()
    {
        InitializeComponent();

        this.DataContext = this;
    }

	private void Button_Click(object sender, RoutedEventArgs e)
	{
        Param += "CL";
	}
}