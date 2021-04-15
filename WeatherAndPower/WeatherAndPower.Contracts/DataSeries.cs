using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
	public class DataSeries
	{
		private static int identificationCounter = 1;


		[JsonIgnore]
		public int Id { get; set; }

		public string Name { get; set; }

		public byte[] Color { get; set; } = { 0, 0, 0 };

		public bool IsSelected { get; set; } = false;

		public bool IsVisible { get; set; } = true;

		public bool IsComparable { get; private set; } = false;

		public DataFormat Format { get; set; }

		public List<Tuple<DateTime, IData>> Series { get; set; }

		public DateTime Minimum
		{
			get { return Series.Min(e => e.Item1); }
		}

		public DateTime Maximum
		{
			get { return Series.Max(e => e.Item1); }
		}

		public DataPoint GetDataPoint(DateTime point)
		{
			if (point <= Maximum && point >= Minimum) {
				//If theres already a matching value for the time in Series use that
				Tuple<DateTime, IData> dp = Series.FirstOrDefault(e => e.Item1 == point);
				if (dp == null) {
					//Otherwise calculate what the in-between value would be
					var closestHigher = Series.Aggregate((closest, next) => {
						var c = closest.Item1.Ticks - point.Ticks;
						var n = next.Item1.Ticks - point.Ticks;
						if (n > 0) {
							if (c > n || c < 0) {
								return next;
							}
						}
						return closest;
					});
					var closestLower = Series.Aggregate((closest, next) =>
					{
						var c = point.Ticks - closest.Item1.Ticks;
						var n = point.Ticks - next.Item1.Ticks;
						if (n > 0) {
							if (c > n || c < 0) {
								return next;
							}
						}
						return closest;
					});

					// Find the position of our point between the two closest dates. value between 0-1
					double pos = (double)(point - closestLower.Item1).Ticks / (closestHigher.Item1 - closestLower.Item1).Ticks;

					// Calculate the would-be value at the position
					var value = ((closestHigher.Item2.Value - closestLower.Item2.Value) * pos) + closestLower.Item2.Value;
					IData idata = (IData)Activator.CreateInstance(Globals.GetTypeFromDataFormat(Format), value);
					dp = new Tuple<DateTime, IData>(point, idata);
				}
				return new DataPoint(Name, Color, Format, dp);
			}
			throw new ArgumentOutOfRangeException("DateTime outside of Series range");
		}

		public void RandomizeColor()
		{
			Color[0] = (byte)Globals.rand.Next(0, 255);
			Color[1] = (byte)Globals.rand.Next(0, 255);
			Color[2] = (byte)Globals.rand.Next(0, 255);

			var luminance = 0.2126 * (float)Color[0] + 0.7152 * (float)Color[1] + 0.0722 * (float)Color[2];
			var average = ((int)Color[0] + (int)Color[1] + (int)Color[2]) / 3;

			if ((Math.Abs((int)Color[0] - average) < 40 &&
				Math.Abs((int)Color[1] - average) < 40 &&
				Math.Abs((int)Color[2] - average) < 40) ||
				(luminance > 205 || luminance < 50)) {
				//Check if RGB values are all very similar, then color is close to grayscale.
				//Also calculate luminance to see if the color is very bright or dark.
				RandomizeColor(); // if either of these is the case, re-randomize a different color
			}
		}

		public void SetColor(byte r, byte g, byte b)
		{
			Color[0] = r;
			Color[1] = b;
			Color[2] = g;
		}

		public DataSeries(string name, DataFormat format, List<Tuple<DateTime, IData>> series)
		{
			Id = identificationCounter++;
			Name = name;
			Format = format;
			Series = series;
			RandomizeColor();
		}

		/**
		 * Only for JSON!
		 */
		[JsonConstructor]
		public DataSeries()
		{
			Id = identificationCounter++;
		}
	}
}
