using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace WeatherAndPower.UI.ValueConverters
{
	[ValueConversion(typeof(long), typeof(TimeSpan))]
	public class EpochToSpanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((TimeSpan)value).Ticks;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var span = new TimeSpan((long)value);
			return span;
		}
	}
}
