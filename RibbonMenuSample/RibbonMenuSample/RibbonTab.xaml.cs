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

namespace RibbonMenuSample
{
    /// <summary>
    /// RibbonTab.xaml の相互作用ロジック
    /// </summary>
    public partial class RibbonTab : TabControl
    {
        #region DependencyProperties

        public static readonly DependencyProperty MenuProperty =
            DependencyProperty.Register
            (
                nameof(Menu),
                typeof(object),
                typeof(RibbonTab),
                new FrameworkPropertyMetadata(null)
            );

        /// <summary>
        /// 「ファイル」メニューを設定するプロパティを取得または設定します。
        /// </summary>
        [System.ComponentModel.Bindable(true)]
        public object Menu
        {
            get { return GetValue(MenuProperty); }
            set { SetValue(MenuProperty, value); }
        }

        #endregion

        public RibbonTab()
        {
            InitializeComponent();
        }
    }
}
