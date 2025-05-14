using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace ValueConverterTest;

public class ParamEx3Converter : Freezable, IValueConverter
{
	public static readonly DependencyProperty ParamProperty =
		DependencyProperty.Register(nameof(Param), typeof(string), typeof(ParamEx3Converter), new PropertyMetadata(""));

	public string Param
	{
		get => (string)GetValue(ParamProperty);
		set => SetValue(ParamProperty, value);
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return $"{value} : {Param}";
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		=> throw new NotImplementedException();

	protected override Freezable CreateInstanceCore()
		=> new ParamEx3Converter();
}
