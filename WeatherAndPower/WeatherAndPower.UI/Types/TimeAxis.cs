using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Media;
using WeatherAndPower.Contracts;

namespace WeatherAndPower.UI
{
	public class TimeAxis : DateTimeAxis
	{
		protected override void PrepareAxisLabel(Control label, object dataContext)
		{
			var lb = label as DateTimeAxisLabel;
			var time = dataContext as DateTime?;
			base.PrepareAxisLabel(lb, dataContext);
			if (lb.IntervalType == DateTimeIntervalType.Hours) {
				if (time?.TimeOfDay.Ticks == 0L) {
					lb.HoursIntervalStringFormat = "{0:t}\n{0:dd-MMM}";
				} else {
					lb.HoursIntervalStringFormat = "{0:t}";
				}
			}
			lb.Margin = new Thickness(3,0,3,0);
			if (VisualTreeHelper.GetChildrenCount(lb) > 0) {
				var text = VisualTreeHelper.GetChild(lb, 0) as TextBlock;
				text.TextAlignment = TextAlignment.Center;
			}
		}

		public TimeAxis()
		{
			ShowGridLines = true;
			Orientation = AxisOrientation.X;
		}
	}
}
