using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BigCalendar.Converters;

public class DateToCountMultiConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values.Length < 2 || values[0] == null || values[1] == null)
		{
			return "0";
		}

		if (values[0] is DateTime date && values[1] is IEnumerable<Data> events)
		{
			var count = events.Count(e => e.Date.Date == date.Date);
			return count.ToString();
		}

		return "0";
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}
