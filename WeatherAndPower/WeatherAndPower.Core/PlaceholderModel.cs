using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAndPower.Contracts;
using WeatherAndPower.Data;

namespace WeatherAndPower.Core
{
    public class PlaceholderModel : AbstractModel, IPlaceholderModel
    {

        private DataPlotModel _DataPlot;
        public DataPlotModel DataPlot
        {
            get { return _DataPlot; }
            private set
            {
                if (_DataPlot != value)
                {
                    _DataPlot = value;
                }
            }
        }
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
            DataPlot.Remove(DataName);
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

        public void SaveChartImage()
		{
			DataPlot.SaveChartImage("test.jpg");
		}

        public void SaveChart()
		{
            DataPlot.SaveChartJson("test.json", 0);
		}

        public void AddPowerDataToPlotAction(PowerType powerType, DateTime startTime, DateTime endTime, string PlotName)
        {
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

        public void FMIAction()
        {

            //FMI.Place = _CityName;
            FMI.Parameters = _DataName;
            FMI.StartTime = "2021-04-20T09:00:00Z";
            FMI.EndTime = "2021-04-20T21:00:00Z";

            //string query = FMI.BuildQuery("observations::weather");
            string query = FMI.BuildQuery("forecast::hirlam::surface::point");
            string request = FMI.BuildRequest(query);
            Console.WriteLine(request);

            var series_list_task = Task.Run(() => FMI.GetData(request));

            try
            {
                series_list_task.Wait();
                var series_list = series_list_task.Result;
                //DataPlot.AddPlot(series_list[0]);
            }
            catch (AggregateException ae)
            {
                Console.WriteLine("FMIAction failed:");
                foreach (var ex in ae.InnerExceptions)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }



        public void AddWeatherGraphAction(string cityName, string parameters, DateTime startTime, DateTime endTime, string plotName, WeatherType.ParameterEnum parameterType)
        {
            FMI.Place = cityName;
            FMI.Parameters = parameters;
            FMI.StartTime = startTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
            FMI.EndTime = endTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
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
                series_list[0].Name = plotName;
                DataPlot.Data.Add(series_list[0]);
            }
            catch (AggregateException ae)
            {
                Console.WriteLine("FMIAction failed:");
                foreach (var ex in ae.InnerExceptions)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public PlaceholderModel(DataPlotModel dataPlot)
        {
            DataPlot = dataPlot;
        }
    }
}
