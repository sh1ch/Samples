using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ValueConverterTest;

public class ParamConverter : IValueConverter
{
	public string Param { get; set; } = "";

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return (value?.ToString() ?? "") + " : " + Param;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		=> throw new NotImplementedException();
}