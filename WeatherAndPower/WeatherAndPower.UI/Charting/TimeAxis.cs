using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.DataVisualization;
using System.Windows.Controls.DataVisualization.Charting;
using WeatherAndPower.Contracts;

namespace WeatherAndPower.UI
{
	public class TimeAxis : DateTimeAxis
	{
		public new TimeSpan? Interval
		{
			get {
				if (base.Interval != null) {
					return new TimeSpan((long)base.Interval);
				} else {
					return null;
				}
			}
			set { base.Interval = value?.Ticks; }
		}
		//public new DateTime? Minimum
		//{
		//	get {
		//		if (base.Minimum != null) {
		//			return new DateTime((long)base.Minimum);
		//		}
		//		else {
		//			return null;
		//		}
		//	}
		//	set { base.Minimum = value?.Ticks; }
		//}

		//public new DateTime? Maximum
		//{
		//	get {
		//		if (base.Maximum != null) {
		//			return new DateTime((long)base.Maximum);
		//		}
		//		else {
		//			return null;
		//		}
		//	}
		//	set { base.Maximum = value?.Ticks; }
		//}
		//public string LabelType
		//{
		//	get { 
		//		if (Interval < new TimeSpan(1, 0, 0)) {
		//			return "t";
		//		}
		//		if (Interval < new TimeSpan(1, 0, 0, 0)) {
		//			return "dd MMM HH:mm";
		//		}
		//		if (Interval < new TimeSpan(180, 0, 0 , 0)) {
		//			return "m";
		//		}
		//		return "d";
		//	}
		//}

		public TimeAxis()
		{
			ShowGridLines = true;
			Orientation = AxisOrientation.X;
			//AxisLabelStyle = (Style)Application.Current.Resources["TimeAxisStyle"];
		}

		//protected override void OnActualRangeChanged(Range<IComparable> range)
		//{
		//	base.OnActualRangeChanged(range);
		//	var span = new TimeSpan((ActualMaximum.Value.Ticks - ActualMinimum.Value.Ticks) / Globals.MaxTimeAxisLabels);
		//	var interval = Globals.TimeIntervalOptions.FirstOrDefault(o => o?.Ticks > span.Ticks);
		//	Interval = interval;
		//}
	}
}
