using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace FixedDecimalPointTextBoxSample
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public double SampleValue { get; set; }  = 12.34;

        public ObservableCollection<SampleData> Samples { get; set; } = new ObservableCollection<SampleData>()
        {
            new SampleData() { Data1 = 23 },
            new SampleData() { Data1 = 34 },
        };

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var datagrid = sender as DataGrid;
            var pressedCell = e.OriginalSource as DataGridCell;
            var key = e.Key;

            if (datagrid == null || pressedCell == null) return;
            if (key == Key.Up || key == Key.Down || key == Key.Left || key == Key.Right) return;

            var nextFocus = pressedCell.PredictFocus(FocusNavigationDirection.Left);
            var inputElement = nextFocus as IInputElement;

            if (inputElement != null && nextFocus != null)
            {
                if (pressedCell.IsAncestorOf(nextFocus))
                {
                    inputElement.Focus();

                    if (inputElement is TextBox)
                    {
                        var target = inputElement as TextBox;
                        var ev = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromVisual(target), e.Timestamp, e.Key);
                        
                        ev.RoutedEvent = e.RoutedEvent;
                        (inputElement as TextBox).RaiseEvent(ev);
                        e.Handled = true;
                    }
                }

                // Enter キーの入力によるフォーカス設定はキー入力のみ無効化する(入力で再度フォーカスが外れるため)
                if (key == Key.Enter)
                {
                    e.Handled = true;
                }
            }
        }

    }
}
