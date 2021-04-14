using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
	public class DataPoint
	{
		public string Name { get; set; }
		public byte[] Color { get; set; }
		public DataFormat Format { get; set; }
		public Tuple<DateTime, IData> Data { get; set; }

		public DataPoint(string name, byte[] color, DataFormat format, Tuple<DateTime, IData> data)
		{
			Name = name;
			Color = color;
			Format = format;
			Data = data;
		}
	}
}
