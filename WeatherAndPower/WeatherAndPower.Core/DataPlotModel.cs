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

		public bool SaveChartImage(string path)
		{
			return Chart.Save(path);
		}

		public bool SaveChartJson(int id, string path)
		{
			var data = Data.FirstOrDefault(p => p.Id == id);
			var writerOptions = new JsonWriterOptions()
			{
				Indented = true,
				SkipValidation = true
			};
			var serializerOptions = new JsonSerializerOptions()
			{
				WriteIndented = true
			};
			FileStream stream;
			try {
				stream = File.Create(path);
			} catch(IOException) {
				return false;
			}
			DataSeriesJsonConverter converter = new DataSeriesJsonConverter();
			using Utf8JsonWriter writer = new Utf8JsonWriter(stream, options: writerOptions);
			converter.Write(writer, data, serializerOptions);
			return true;
		}

		public DataPlotModel(ICustomChart chart)
		{
			Chart = chart;
		}
	}
}