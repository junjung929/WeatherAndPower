using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
	public class DataSeries
	{
		public int Id { get; set; } = 0;

		public string Name { get; set; }

		public DataFormat Format { get; set; }

		public List<Tuple<DateTime, IData>> Series { get; set; }

		public DataSeries(string name, DataFormat format, List<Tuple<DateTime, IData>> series)
		{
			Name = name;
			Series = series;
			Format = format;
		}
	}
}
