using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace WeatherAndPower.UI.ValueConverters
{
	[ValueConversion(typeof(byte[]), typeof(Brush))]
	public class BytesToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var bytecolor = value as byte[];
			return new SolidColorBrush(Color.FromRgb(bytecolor[0], bytecolor[1], bytecolor[2]));
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var color = (SolidColorBrush)value;
			return new byte[] { color.Color.R, color.Color.B, color.Color.G };
		}
	}
}
