using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAndPower.Contracts;

namespace WeatherAndPower.Core
{
	public class DataPlotModel : AbstractModel, IDataPlotModel
	{
		public ICustomChart Chart { get; set; }
		public ObservableCollection<DataSeries> Data { get; }
			= new ObservableCollection<DataSeries>();

		private List<DateTime> _CreateTimeSeries(DateTime from, DateTime to, int count)
		{
			TimeSpan interval = new TimeSpan(to.Subtract(from).Ticks / count);
			var list = new List<DateTime>();
			for (var i = 0; i < count; i++) {
				list.Add(from.Add(new TimeSpan(interval.Ticks * i)));
			}
			return list;
		}
		private List<Temperature> _GenerateRandomPlot(double min, double max, double count)
		{
			var list = new List<Temperature>();
			for (var i = 0; i < count; i++) {
				var number = (Globals.rand.Next() % (max - min)) + min;
				list.Add(new Temperature(number));
			}
			return list;
		}

		public void AddRandomPlot(string name)
		{
			var x = _CreateTimeSeries(
				DateTime.Now.Subtract(new TimeSpan(0,12,0,0)),
				DateTime.Now,
				48);
			var y = _GenerateRandomPlot(0, 400, 48);

			var series = x.Zip(y, (_x, _y) => new Tuple<DateTime, IData>(_x, _y)).ToList();
			Data.Add(new DataSeries(name, DataFormat.Temperature, series));
		}

		public void Clear()
		{
			Data.Clear();
		}

		public void Remove(string name)
		{
			while (Data.Where(i => i.Name == name).Count() > 0) {
				var item = Data.First(i => i.Name == name);
				Data.Remove(item);
			}
		}

		public bool SaveChart(string path)
		{
			return Chart.Save(path);
		}

		public DataPlotModel(ICustomChart chart)
		{
			Chart = chart;
		}
	}
}