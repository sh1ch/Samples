using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace RadioEnumBindingTest;

public class BoolToEnumConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value == null || parameter == null)
		{
			return DependencyProperty.UnsetValue;
		}

		if (Enum.IsDefined(value.GetType(), value) == false)
		{
			return DependencyProperty.UnsetValue;
		}

		var isResult = false;
		var canParse = Enum.TryParse(value.GetType(), parameter.ToString(), out object? result);

		if (canParse && result != null)
		{
			isResult = (int)result == (int)value;
		}
		else
		{
			isResult = false;
		}

		return isResult;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		var parameterText = parameter?.ToString() ?? "";

		if (parameterText == "")
		{
			return DependencyProperty.UnsetValue;
		}

		return Enum.Parse(targetType, parameterText);
	}
}
