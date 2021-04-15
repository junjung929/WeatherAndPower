using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WeatherAndPower.UI.ValueConverters
{
	[ValueConversion(typeof(bool), typeof(int))]
	public class BoolToIntConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is string) {
				if (((string)value).ToLower() == "true" || ((string)value).ToLower() == "false") {
					value = System.Convert.ToBoolean(value);
				}
				else if (((string)value) == "1" || ((string)value) == "0") {
					value = System.Convert.ToBoolean(System.Convert.ToInt32((string)value));
				}
			}

			return value.Equals(true) ? 1 : 0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value.Equals(1);
		}
	}
}
