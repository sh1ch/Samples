using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataGridColumnPositionSaveSample
{
    /// <summary>
    /// <see cref="TagBehavior"/> クラスは、添付プロパティを定義するクラスです。
    /// <para>
    /// コントロールに（自由な用途の）プロパティ <see cref="Tag"/> を与えます。
    /// </para>
    /// </summary>
    public static class TagBehavior
    {
        #region Properties

        public static readonly DependencyProperty TagProperty =
            DependencyProperty.RegisterAttached(
                "Tag",
                typeof(object),
                typeof(TagBehavior),
                new FrameworkPropertyMetadata(null)
            );

        public static object GetTag(DependencyObject d)
        {
            return d.GetValue(TagProperty);
        }

        public static void SetTag(DependencyObject d, object value)
        {
            d.SetValue(TagProperty, value);
        }

        #endregion

    }
}
