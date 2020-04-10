using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using TextResources = PrintSample.Properties.Resources;

namespace PrintSample
{
    /// <summary>
    /// <see cref="DuplexingToTextConverter"/> クラスは、カスタム ロジックをバインディングに適用する方法を提供します。 
    /// <para>
    /// <see cref="Duplexing"/> の値と対応するテキストを相互に変換します。
    /// </para>
    /// </summary>
    public class DuplexingToTextConverter : IValueConverter
    {
        #region Public Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.ToString() == "")
            {
                return TextResources.Duplexing_OneSided;
            }

            if (Enum.IsDefined(value.GetType(), value) == false)
            {
                return TextResources.Duplexing_OneSided;
            }

            var str = value.ToString();
            var duplexing = (Duplexing)Enum.Parse(value.GetType(), str);

            var text = TextResources.Duplexing_OneSided;

            if (duplexing == Duplexing.OneSided) text = TextResources.Duplexing_OneSided;
            else if (duplexing == Duplexing.TwoSidedLongEdge) text = TextResources.Duplexing_TwoSidedLongEdge;
            else if (duplexing == Duplexing.TwoSidedShortEdge) text = TextResources.Duplexing_TwoSidedShortEdge;

            return text;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Duplexing.Unknown;
            }

            var duplexing = Duplexing.OneSided;
            var duplexingText = value.ToString();

            if (duplexingText == TextResources.Duplexing_OneSided) duplexing = Duplexing.OneSided;
            else if (duplexingText == TextResources.Duplexing_TwoSidedLongEdge) duplexing = Duplexing.TwoSidedLongEdge;
            else if (duplexingText == TextResources.Duplexing_TwoSidedShortEdge) duplexing = Duplexing.TwoSidedShortEdge;

            return duplexing;
        }

        #endregion

    }
}
