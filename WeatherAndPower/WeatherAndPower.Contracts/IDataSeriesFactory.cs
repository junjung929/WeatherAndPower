using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
	public interface IDataSeriesFactory
	{
		IDataSeries CreateDataSeries(string name, DataFormat format, List<Tuple<DateTime, IData>> series);
	}
}
