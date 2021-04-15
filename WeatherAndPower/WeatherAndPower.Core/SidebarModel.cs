using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAndPower.Contracts;
using WeatherAndPower.Data;

namespace WeatherAndPower.Core
{
	public class SidebarModel : AbstractModel, ISidebarModel
	{
        public DataPlotModel DataPlot { get; set; }

        public IWindowFactory WindowFactory { get; set; }

        private int _PlaceholderProperty = 0;
        public int PlaceholderProperty
        {
            get
            {
                return _PlaceholderProperty;
            }
            private set
            {
                if (_PlaceholderProperty != value)
                {
                    _PlaceholderProperty = value;
                }
            }
        }
		public ObservableCollection<DataSeries> Data
		{
			get {
				return DataPlot.Data;
			}
		}

		public void AddPowerDataToPlotAction(PowerType powerType, DateTime startTime, DateTime endTime, string plotName)
		{
            var series_task = Task.Run(() => Fingrid.Get(powerType, startTime, endTime));

            try
            {
                series_task.Wait();
                var series = series_task.Result;
                series.Name = plotName;
                DataPlot.Data.Add(series);
            }
            catch (AggregateException e)
            {
                Console.WriteLine("Cannot add data to plot");
                foreach (var ex in e.InnerExceptions)
                {
                    Console.WriteLine(ex.Message);
                }
                throw new Exception("Cannot add data to plot");
            }
		}

		public void AddWeatherGraphAction(string cityName, string parameters, DateTime startTime, DateTime endTime, string plotName, WeatherType.ParameterEnum parameterType)
		{
			throw new NotImplementedException();
		}

		public void ClearGraph()
		{
			DataPlot.Clear();
		}

		public async void CompareData()
		{
            DateTime point = await DataPlot.Pick();
            var data = DataPlot.Data.Where(d => {
                return (d.Format == DataFormat.Power && d.Maximum > point && d.Minimum < point);
            }).Select(d => d.GetDataPoint(point));

            if (data.Count() > 0) {
				var model = new PieModel();
				foreach (var d in data) {
					model.Data.Add(d);
				}
				WindowFactory.CreateWindow(model);
			}
		}

		public void OpenData(string path)
		{
			DataPlot.LoadChartJson(path);
		}

		public void RemoveData(int id)
		{
			DataPlot.Remove(id);
		}

		public void SaveData(string path, params int[] ids)
		{
			DataPlot.SaveChartJson(path, ids);
		}

		public void SaveDataImage(string path)
		{
			DataPlot.SaveChartImage(path);
		}

		public SidebarModel(DataPlotModel dataPlot, IWindowFactory windowFactory)
		{
			DataPlot = dataPlot;
			WindowFactory = windowFactory;
		}
	}
}
