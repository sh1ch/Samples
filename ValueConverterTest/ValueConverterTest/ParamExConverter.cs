using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace ValueConverterTest;

public class ParamExConverter : MarkupExtension, IValueConverter
{
	public string Param { get; set; } = "";

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return (value?.ToString() ?? "") + " : " + Param;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		=> throw new NotImplementedException();
}
