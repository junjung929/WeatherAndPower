using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.DataVisualization.Charting;

namespace WeatherAndPower.UI
{
	public class TimeAxis : LinearAxis
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
		public new DateTime? Minimum
		{
			get {
				if (base.Minimum != null) {
					return new DateTime((long)base.Minimum);
				}
				else {
					return null;
				}
			}
			set { base.Minimum = value?.Ticks; }
		}

		public new DateTime? Maximum
		{
			get {
				if (base.Maximum != null) {
					return new DateTime((long)base.Maximum);
				}
				else {
					return null;
				}
			}
			set { base.Maximum = value?.Ticks; }
		}
	}
}
