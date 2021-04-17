using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAndPower.Contracts;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace WeatherAndPower.Core
{
	public class DataPlotModel : AbstractModel, IDataPlotModel
	{
		public ICustomChart Chart { get; set; }
		public ObservableCollection<IDataSeries> Data { get; }
			= new ObservableCollection<IDataSeries>();

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
			var count = 500;
			var x = _CreateTimeSeries(
				DateTime.Now.Subtract(new TimeSpan(0,24,0,0)),
				DateTime.Now,
				count);
			var y = _GenerateRandomPlot(0, 400, count);

			var series = x.Zip(y, (_x, _y) => new Tuple<DateTime, IData>(_x, _y)).ToList();
			Data.Add(new DataSeries(name, DataFormat.Temperature, series));
		}

		public void Clear()
		{
			Data.Clear();
		}

		public void Remove(int id)
		{
			while (Data.Where(i => i.Id == id).Count() > 0) {
				var item = Data.First(i => i.Id == id);
				Data.Remove(item);
			}
		}

		public bool SaveChartImage(string path)
		{
			return Chart.Save(path);
		}

		public bool SaveChartJson(string path, params int[] ids)
		{
			var data = Data.Where(p => ids.Contains(p.Id)).ToArray();

			try {
				var options = new JsonSerializerOptions()
				{
					WriteIndented = true
				};
				options.Converters.Add(new IDataJsonConverter());
				options.Converters.Add(new ByteColorJsonConverter());
				File.WriteAllText(path, JsonSerializer.Serialize(data, options: options));
			} catch(IOException) {
				return false;
			}

			return true;
		}

		public bool LoadChartJson(string path)
		{
			var jsonString = File.ReadAllText(path);
			var options = new JsonSerializerOptions();
			options.Converters.Add(new IDataJsonConverter());
			options.Converters.Add(new ByteColorJsonConverter());
			var data = JsonSerializer.Deserialize<DataSeries[]>(jsonString, options: options);

			foreach (DataSeries d in data) {
				Data.Add(d);
			}

			return true;
		}

		public async Task<DateTime> Pick()
		{
			return await Chart.Pick();
		}

		public DataPlotModel(ICustomChart chart)
		{
			Chart = chart;
		}
	}
}