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

		public DataFormat Format { get; set; }

		public List<Tuple<DateTime, IData>> Series { get; set; }

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
				//Also calculate luminance to see if the color is very bright. 
				RandomizeColor(); // if either of these is the case, re-randomize a different color
			}
		}
		public void SetColor(byte r, byte g, byte b)
		{
			Color[0] = r;
			Color[1] = b;
			Color[2] = g;
		}

		public void SetId()
		{

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
