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
		public ObservableCollection<IDataSeries> Data
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
            FMI.Place = cityName;
            FMI.Parameters = parameters;
            //FMI.StartTime = startTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
            //FMI.EndTime = endTime.ToString("yyyy-MM-ddTHH:mm:ssZ");


            if (TimeHandler.ForecastTooFar(startTime))
            {
                return;
            }

            FMI.StartTime = TimeHandler.ConvertToLocalTime(startTime).ToString("yyyy-MM-ddTHH:mm:ssZ");
            FMI.EndTime = TimeHandler.ConvertToLocalTime(endTime).ToString("yyyy-MM-ddTHH:mm:ssZ");

            FMI.Timestep = "10";

            int.TryParse(FMI.Timestep, out int timestep);

            if (TimeHandler.DataTooBig(startTime, endTime, timestep))
            {
                return;
            }


            string query;

            if (parameterType == WeatherType.ParameterEnum.Forecast)
            {
                query = FMI.BuildQuery("forecast::hirlam::surface::point");
            }
            else
            {
                query = FMI.BuildQuery("observations::weather");
            }
            string request = FMI.BuildRequest(query);
            Console.WriteLine(request);

            var series_list_task = Task.Run(() => FMI.GetData(request));
            try
            {
                series_list_task.Wait();
                var series_list = series_list_task.Result;
                foreach (var series in series_list)
                {
                    series.Name = plotName;
                    DataPlot.Data.Add(series);
                }

            }
            catch (AggregateException ae)
            {
                Console.WriteLine("FMIAction failed:");
                foreach (var ex in ae.InnerExceptions)
                {
                    Console.WriteLine(ex.Message);
                    throw new Exception(ex.Message);
                }
            }
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

        public void AddData()
		{
            WindowFactory.CreateWindow(new AddWindowModel(DataPlot));
		}

		public void OpenData(string path)
		{
			DataPlot.LoadChartJson(path);
		}

		public void RemoveSelectedData()
		{
            var ids = DataPlot.Data.Where(e => e.IsSelected).Select(e => e.Id).ToArray();
            if (ids.ToList().Count <1)
            {
                throw new Exception("Please select at least one data from the list");
            }
            foreach (var id in ids) {
                DataPlot.Remove(id);
			}
		}

        public void SaveSelectedData(string path)
		{
            var ids = DataPlot.Data.Where(e => e.IsSelected).Select(e => e.Id).ToArray();
            if (ids.Count() > 0) {
                DataPlot.SaveChartJson(path, ids);
            }
            else
            {
                throw new Exception("Please select at least one data from the list");
            }
		}

		public void SaveData(string path, params int[] ids)
		{
			DataPlot.SaveChartJson(path, ids);
		}

		public void SaveDataImage(string path)
		{
			DataPlot.SaveChartImage(path);
		}
        public IAddWindowModel CreateNewAddWindow()
        {
            return new AddWindowModel(DataPlot);
        }

		public SidebarModel(DataPlotModel dataPlot, IWindowFactory windowFactory)
		{
			DataPlot = dataPlot;
			WindowFactory = windowFactory;
		}
	}
}
