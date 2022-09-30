using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
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

namespace CheckBoxToByteTest;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public string ByteText { get; set; } = "FF";

    public byte Result
    {
        get
        {
            var bools = new bool[8];

            bools[0] = IsEnabled1;
            bools[1] = IsEnabled2;
            bools[2] = IsEnabled3;
            bools[3] = IsEnabled4;
            bools[4] = IsEnabled5;
            bools[5] = IsEnabled6;
            bools[6] = IsEnabled7;
            bools[7] = IsEnabled8;

            return ToByte(bools);
        }
        set
        {
            var bools = ToBools(value);

            IsEnabled1 = bools[0];
            IsEnabled2 = bools[1];
            IsEnabled3 = bools[2];
            IsEnabled4 = bools[3];
            IsEnabled5 = bools[4];
            IsEnabled6 = bools[5];
            IsEnabled7 = bools[6];
            IsEnabled8 = bools[7];
        }
    }

    private bool _IsEnabled1 = true;
    public bool IsEnabled1
    {
        get => _IsEnabled1;
        set
        {
            _IsEnabled1 = value;
            Notify(nameof(IsEnabled1));
        }
    }

    #region Other PropertyChanged

    private bool _IsEnabled2;
    public bool IsEnabled2
    {
        get => _IsEnabled2;
        set
        {
            _IsEnabled2 = value;
            Notify(nameof(IsEnabled2));
        }
    }

    private bool _IsEnabled3;
    public bool IsEnabled3
    {
        get => _IsEnabled3;
        set
        {
            _IsEnabled3 = value;
            Notify(nameof(IsEnabled3));
        }
    }

    private bool _IsEnabled4;
    public bool IsEnabled4
    {
        get => _IsEnabled4;
        set
        {
            _IsEnabled4 = value;
            Notify(nameof(IsEnabled4));
        }
    }

    private bool _IsEnabled5;
    public bool IsEnabled5
    {
        get => _IsEnabled5;
        set
        {
            _IsEnabled5 = value;
            Notify(nameof(IsEnabled5));
        }
    }

    private bool _IsEnabled6;
    public bool IsEnabled6
    {
        get => _IsEnabled6;
        set
        {
            _IsEnabled6 = value;
            Notify(nameof(IsEnabled6));
        }
    }

    private bool _IsEnabled7;
    public bool IsEnabled7
    {
        get => _IsEnabled7;
        set
        {
            _IsEnabled7 = value;
            Notify(nameof(IsEnabled7));
        }
    }

    private bool _IsEnabled8;
    public bool IsEnabled8
    {
        get => _IsEnabled8;
        set
        {
            _IsEnabled8 = value;
            Notify(nameof(IsEnabled8));
        }
    }

    #endregion

    public MainWindow()
    {
        InitializeComponent();

        PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName != nameof(Result))
            {
                Notify(nameof(Result));
            }
        };

        DataContext = this;
    }

    private void WriteConsole()
    {
        Debug.WriteLine("test");
    }

    private bool[] ToBools(byte data)
    {
        var bools = new bool[8];

        for (int i = 0; i < 8; i++)
        {
            bools[i] = (data & (1 << i)) != 0;
        }

        Array.Reverse(bools);

        return bools;
    }

    private byte ToByte(bool[] sources)
    {
        byte result = 0x00;
        int index = 8 - sources.Length;

        if (sources.Length <= 0)
        {
            return result;
        }

        foreach (var source in sources)
        {
            if (source)
            {
                result |= (byte)(1 << (7 - index));
            }

            index++;
        }

        return result;
    }

    private void Notify(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var canParse = byte.TryParse(ByteText, NumberStyles.HexNumber, NumberFormatInfo.CurrentInfo, out byte result);

        if (canParse)
        {
            Result = result;
        }
    }
}
