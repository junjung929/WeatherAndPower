using System;
using System.Threading.Tasks;
using WeatherAndPower.Contracts;
using WeatherAndPower.Data;
using System.Collections.Generic;

namespace WeatherAndPower.Core
{
    class AddWindowModel : AbstractModel, IAddWindowModel
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


        public void AddGraphAction(IAddWindowModel.DataTypeEnum dataType)
        {
            throw new NotImplementedException();
        }

        public void AddPowerGraphAction(PowerType powerType, DateTime startTime, DateTime endTime, string graphName)
        {

            if (powerType == null)
            {
                throw new Exception("Please choose the category");
            }
            if (TimeHandler.DataTooBig(startTime, endTime, powerType.Interval))
            {
                return;
            }
            startTime = TimeHandler.ConvertToLocalTime(startTime);
            endTime = TimeHandler.ConvertToLocalTime(endTime);

            try
            {
                IsTimeValid(startTime, endTime);
                IsPlotNameValid(graphName);
                var series_task = Task.Run(() => Fingrid.Get(powerType, startTime, endTime));

                try
                {
                    series_task.Wait();
                    var series = series_task.Result;
                    series.Name = graphName + " (" + powerType.Source + ")";
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
            catch (Exception e)
            {

                throw e;
            }

        }

        public void AddWeatherGraph(string cityName, string parameters, DateTime startTime, DateTime endTime,
            string graphName, WeatherType.ParameterEnum parameterType, int interval)
        {
            if (cityName == null || cityName == "")
            {
                throw new Exception("Please give a name of cities in Finland");
            }

            List<Tuple<DateTime, DateTime>> timepairs = TimeHandler.SplitFMIRequest(startTime, endTime);

            if (TimeHandler.ForecastTooFar(startTime)) { return; }

            string step = interval.ToString();
            FMI.Timestep = step;
            if (TimeHandler.DataTooBig(startTime, endTime, interval)) { return; }

            Dictionary<string, IDataSeries> combined_graphs = new Dictionary<string, IDataSeries>();

            foreach (var timepair in timepairs)
            {
                FMI.StartTime = TimeHandler.ConvertToLocalTime(timepair.Item1).ToString("yyyy-MM-ddTHH:mm:ssZ");
                FMI.EndTime = TimeHandler.ConvertToLocalTime(timepair.Item2).ToString("yyyy-MM-ddTHH:mm:ssZ");
                try
                {
                    IsTimeValid(startTime, endTime);
                    IsPlotNameValid(graphName);

                    FMI.Place = cityName;
                    FMI.Parameters = parameters;                   

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
                            AddToDict(ref combined_graphs, series);
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
                catch (Exception e)
                {
                    throw e;
                }
            }
            // plotting here
            foreach(var graph in combined_graphs)
            {
                graph.Value.Name = graphName + " (" + graph.Value.Name + ")";
                DataPlot.Data.Add(graph.Value);
            }
        }

        public IPowerInputModel CreateNewPowerInputModel()
        {
            return new PowerInputModel();
        }

        public IWeatherInputModel CreateNewWeatherInputModel()
        {
            return new WeatherInputModel();
        }

        private Boolean IsTimeValid(DateTime startTime, DateTime endTime)
        {
            if (startTime.Equals(endTime)
                    || DateTime.Compare(startTime, endTime) > 0)
            {
                throw new Exception("Please choose valid time range");
            }
            return true;
        }
        private Boolean IsPlotNameValid(string plotName)
        {
            if (plotName == null || plotName == "")
            {
                throw new Exception("Please type the name of new plot");
            }
            return true;
        }

        public AddWindowModel(DataPlotModel dataPlot)
        {
            DataPlot = dataPlot;
        }

        public void AddToDict(ref Dictionary<string, IDataSeries> dict, IDataSeries plot)
        {
            if (dict.ContainsKey(plot.Name))
            {
                var series = dict[plot.Name];
                series.Series.AddRange(plot.Series);
            }
            else
            {
                dict.Add(plot.Name, plot);
            }
        }
    }
}
