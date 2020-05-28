using System;
using System.Collections.Generic;
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

namespace DataGridCustomCheckBoxTest
{
    public class TwoCheckBox : Control
    {
        #region DependencyProperties

        public static readonly DependencyProperty IsChecked1Property =
            DependencyProperty.Register(
                nameof(IsChecked1),
                typeof(bool),
                typeof(TwoCheckBox),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
            );

        public bool IsChecked1
        {
            get { return (bool)GetValue(IsChecked1Property); }
            set { SetValue(IsChecked1Property, value); }
        }

        public static readonly DependencyProperty IsChecked2Property =
            DependencyProperty.Register(
                nameof(IsChecked2),
                typeof(bool),
                typeof(TwoCheckBox),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
            );

        public bool IsChecked2
        {
            get { return (bool)GetValue(IsChecked2Property); }
            set { SetValue(IsChecked2Property, value); }
        }

        #endregion

        static TwoCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TwoCheckBox), new FrameworkPropertyMetadata(typeof(TwoCheckBox)));
        }
    }
}
