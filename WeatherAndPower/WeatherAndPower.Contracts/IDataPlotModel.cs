using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
	public interface IDataPlotModel
	{
		ObservableCollection<DataSeries> Data { get; }

		void Clear();

		void Remove(string name);

		bool SaveChart(string path);
	}
}
