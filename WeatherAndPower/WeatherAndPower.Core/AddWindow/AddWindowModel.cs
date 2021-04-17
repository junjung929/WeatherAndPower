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

        private IPreference _Preference { get; set; }
        public IPreference Preference
        {
            get { return _Preference; }
            set
            {
                _Preference = value;
                NotifyPropertyChanged("Preference");
            }
        }

        public void AddPowerGraphAction()
        {
            var preference = Preference as IPowerPreference;
            if (preference.PowerType == null)
            {
                throw new Exception("Please choose the category");
            }
            if (TimeHandler.DataTooBig(preference.StartTime, preference.EndTime, preference.Interval.Value))
            {
                return;
            }
             var startTime = TimeHandler.ConvertToLocalTime(preference.StartTime);
            var endTime = TimeHandler.ConvertToLocalTime(preference.EndTime);

            try
            {
                TimeHandler.IsTimeValid(startTime, endTime);
                IsPlotNameValid(preference.PlotName);
                var series_task = Task.Run(() => Fingrid.Get(preference.PowerType, startTime, endTime));

                try
                {
                    series_task.Wait();
                    var series = series_task.Result;
                    series.Name = preference.PlotName + " (" + preference.PowerType.Source + ")";
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
                TimeHandler.IsTimeValid(startTime, endTime);
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

        public IPowerInputModel CreateNewPowerInputModel(IPowerPreference preference)
        {
            return new PowerInputModel(preference);
        }
        public IWeatherInputModel CreateNewWeatherInputModel(IWeatherPreference preference)
        {
            return new WeatherInputModel(preference);
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
            Preference = new PowerPreference();
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

        public IPowerPreference CreateNewPowerPreference()
        {
            return new PowerPreference();
        }

        
    }
}

