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
    public class PlaceholderModel : AbstractModel, IPlaceholderModel
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

        private string _DataName;
        public string DataName
        {
            get { return _DataName; }
            set
            {
                if (_DataName != value)
                {
                    _DataName = value;
                    NotifyPropertyChanged("DataName");
                }
            }
        }

        public void PlaceholderAction1()
        {
            DataPlot.AddRandomPlot(DataName);

        }

        public void PlaceholderAction2()
        {
            DataPlot.Clear();
        }

        public void PlaceholderAction3()
        {
            //DataPlot.Remove(DataName);
        }

        public void PlaceholderAction5()
        {
            var seires = Task.Run(() => Fingrid.Get(
                PowerType.WindPowerProdRT,
                new DateTime(2021, 3, 14, 2, 00, 00),
                new DateTime(2021, 3, 14, 5, 00, 00)))
                .Result;
            DataPlot.Data.Add(seires);
        }

        public bool SaveChartImage(string path)
        {
            return DataPlot.SaveChartImage(path);
        }

        public bool SaveChart(string path, params int[] ids)
        {
            return DataPlot.SaveChartJson(path, ids);
        }

        public bool LoadChart(string path)
        {
            return DataPlot.LoadChartJson(path);
        }

        public async void ComparePowers()
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

        public void AddPowerDataToPlotAction(PowerType powerType, DateTime startTime, DateTime endTime, string PlotName)
        {
            // If user choses bot to proceed with a large dataset
            if (TimeHandler.DataTooBig(startTime, endTime, powerType.Interval))
            {
                return;
            }

            startTime = TimeHandler.ConvertToLocalTime(startTime);
            endTime = TimeHandler.ConvertToLocalTime(endTime);

            var series_task = Task.Run(() => Fingrid.Get(powerType, startTime, endTime));

            try
            {
                series_task.Wait();
                var series = series_task.Result;
                series.Name = PlotName;
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

        public IAddWindowModel CreateNewAddWindow()
        {
            return new AddWindowModel(DataPlot);
        }

        public PlaceholderModel(DataPlotModel dataPlot)
        {
            DataPlot = dataPlot;
            WindowFactory = windowFactory;
        }
    }
}
