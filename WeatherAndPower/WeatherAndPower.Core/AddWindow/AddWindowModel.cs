using System;
using System.Threading.Tasks;
using WeatherAndPower.Contracts;
using WeatherAndPower.Data;

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
                    series.IsComparable = powerType.Source != PowerType.SourceEnum.All;
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
            Dictionary<string, IDataSeries> combined_graphs = new Dictionary<string, IDataSeries>();
            try
            {
                IsTimeValid(startTime, endTime);
                IsPlotNameValid(graphName);
                combined_graphs = FMI.GetAllData(startTime, endTime, interval, graphName, cityName, parameters, parameterType);
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
            foreach (var graph in combined_graphs)
            {
                graph.Value.Name = graphName + " (" + graph.Value.Name + ")";
                DataPlot.Data.Add(graph.Value);
            }
        }
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
    }
}
