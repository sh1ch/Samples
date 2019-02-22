using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace DecimalValueRoundTest
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Properties

        private string _InputValue;

        public string InputValue
        {
            get { return _InputValue; }
            set
            {
                _InputValue = value;

                Update(value);
                RaisePropertyChanged(nameof(InputValue));
            }
        }

        private string _RoundValueEven;

        public string RoundValueEven
        {
            get { return _RoundValueEven; }
            set
            {
                _RoundValueEven = value;
                RaisePropertyChanged(nameof(RoundValueEven));
            }
        }

        private string _RoundValueZero;

        public string RoundValueZero
        {
            get { return _RoundValueZero; }
            set
            {
                _RoundValueZero = value;
                RaisePropertyChanged(nameof(RoundValueZero));
            }
        }

        private int _RoundPos;

        public int RoundPos
        {
            get { return _RoundPos; }
            set
            {
                _RoundPos = value;

                Update(InputValue);
                RaisePropertyChanged(nameof(RoundPos));
            }
        }

        #endregion

        public MainWindow()
        {
            InitializeComponent();

            // 初期値
            InputValue = "100.2345";
            RoundPos = 3;

            this.DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void Update(string value)
        {
            double r = 0;
            if (double.TryParse(value, out r))
            {
                var r1 = Math.Round(r, RoundPos, MidpointRounding.ToEven).ToString($"F{RoundPos}");
                var r2 = Math.Round(r, RoundPos, MidpointRounding.AwayFromZero).ToString($"F{RoundPos}");

                RoundValueEven = r1;
                RoundValueZero = r2;
            }
        }
    }
}
