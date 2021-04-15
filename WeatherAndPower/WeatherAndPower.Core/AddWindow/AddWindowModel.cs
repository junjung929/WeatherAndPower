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
            try
            {
                IsTimeValid(startTime, endTime);
                IsPlotNameValid(graphName);
                var series_task = Task.Run(() => Fingrid.Get(powerType, startTime, endTime));

                try
                {
                    series_task.Wait();
                    var series = series_task.Result;
                    series.Name = graphName;
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

        public void AddWeatherGraph(string cityName, string parameters, DateTime startTime, DateTime endTime, string graphName, WeatherType.ParameterEnum parameterType)
        {
            throw new NotImplementedException();
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
