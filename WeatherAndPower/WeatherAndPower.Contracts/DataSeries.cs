using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
	public class DataSeries
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public List<Tuple<DateTime, IData>> Series { get; set; }

		public DataSeries(string name, List<Tuple<DateTime, IData>> series)
		{
			Id = 0; //This will be set when the series is added to a plot
			Name = name;
			Series = series;
		}
	}
}
