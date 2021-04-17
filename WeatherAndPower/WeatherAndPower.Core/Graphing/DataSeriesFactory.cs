using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAndPower.Contracts;

namespace WeatherAndPower.Core
{
	public class DataSeriesFactory : IDataSeriesFactory
	{
		public IDataSeries CreateDataSeries(string name, DataFormat format, List<Tuple<DateTime, IData>> series)
		{
			return new DataSeries(name, format, series);
		}
	}
}
