using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WeatherAndPower.UI.ValueConverters
{
	[ValueConversion(typeof(bool), typeof(Visibility))]
	public class BoolToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value is string) {
				if (((string)value).ToLower() == "true" || ((string)value).ToLower() == "false") {
					value = System.Convert.ToBoolean(value);
				}
				else if (((string)value) == "1" || ((string)value) == "0") {
					value = System.Convert.ToBoolean(System.Convert.ToInt32((string)value));
				}
			}

			return value.Equals(true) ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value.Equals(Visibility.Visible);
		}
	}
}
