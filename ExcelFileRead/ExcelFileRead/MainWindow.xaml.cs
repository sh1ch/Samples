using ClosedXML.Excel;
using Microsoft.Win32;
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

namespace ExcelFileRead;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        string filePath = "";
        string address = "";

        filePath = FilePath.Text;
        address = CellAddress.Text;

        var result = await ReadAsync(filePath, address);

        Result.Content = result; 
    }

    private async Task<string> ReadAsync(string filePath, string address)
    {
        return await Task.Run(() => Read(filePath, address));
    }

    private string Read(string filePath, string address)
    {
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var book = new XLWorkbook(stream);

        var sheet = book.Worksheets.FirstOrDefault();

        if (sheet == null) return "";

        var cell = sheet.Cell(address);

        return cell?.GetValue<string>() ?? "";
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog();

        dialog.FileName = "";
        dialog.DefaultExt = ".xlsx";
        dialog.Filter = "Excel Files (.xlsx)|*.xlsx";
        dialog.Multiselect = false;

        if (dialog.ShowDialog() ?? false)
        {
            var selectedFileName = dialog.FileName;

            FilePath.Text = selectedFileName;
        }
    }
}
