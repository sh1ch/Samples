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

namespace PrintSample
{
    /// <summary>
    /// PrintPreview.xaml の相互作用ロジック
    /// </summary>
    public partial class PrintPreview : UserControl
    {
        #region DependencyProperties

        public static readonly DependencyProperty OptionProperty =
            DependencyProperty.Register
            (
                nameof(OptionContent),
                typeof(object),
                typeof(PrintPreview),
                new  UIPropertyMetadata(null)

            );

        public object OptionContent
        {
            get { return GetValue(OptionProperty); }
            set { SetValue(OptionProperty, value); }
        }

        #endregion

        public PrintPreview()
        {
            InitializeComponent();
        }
    }
}
