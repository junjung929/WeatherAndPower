using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WeatherAndPower.Contracts;

namespace WeatherAndPower.Core
{
    public class PowerInputModel : AbstractModel, IPowerInputModel
    {
        public ObservableCollection<PowerType> PowerTypes { get; } = new ObservableCollection<PowerType>(PowerType.GetAll<PowerType>());

        public List<Interval> Intervals { get; set; } = new List<Interval>()
        {
            new Interval(3),
            //new Interval(30),
            new Interval(60),
            //new Interval(360),
            //new Interval(720),
            //new Interval(1440),
            //new Interval(1440*7),
            //new Interval(1440*30)
        };
        private IPowerPreference _Preference { get; set; }
        public IPowerPreference Preference
        {
            get { return _Preference; }
            set
            {
                _Preference = value;
                NotifyPropertyChanged("Preference");
            }
        }

        public ObservableCollection<PowerType.SourceEnum> PowerSources { get; } = new ObservableCollection<PowerType.SourceEnum>();
        public ObservableCollection<PowerType.ServiceEnum> PowerServices { get; } = new ObservableCollection<PowerType.ServiceEnum>();


        public ObservableCollection<PowerType.ParameterEnum> PowerParameters { get; } = new ObservableCollection<PowerType.ParameterEnum>();

        public bool CheckIsRealTimeParameter(PowerType.ParameterEnum powerParameter)
        {
            if (powerParameter == PowerType.ParameterEnum.RealTime) return true;
            return false;
        }

        public IDateTimeInputModel CreateDateTimeInputModel(IPreference preference)
        {
            return new DateTimeInputModel(preference);
        }

        public List<Interval> GetUpdatedIntervals(int minInterval)
        {
            var intervals = new List<Interval>();
            Intervals.ToList().ForEach(interval =>
            {
                if (interval.Value < minInterval)
                {
                    intervals.Add(new Interval(interval.Value) { IsEnabled = false });
                }
                else
                {
                    intervals.Add(new Interval(interval.Value));
                }
            });
            return intervals;
        }

        public void UpdatePowerSources()
        {
            if (PowerSources.Count != 0)
            {
                PowerSources.Clear();
            }
            PowerTypes.Select(e => e.Source).Distinct().ToList().ForEach(e =>
            {
                PowerSources.Add(e);
            });
        }

        public void UpdatePowerServices()
        {
            if (PowerServices.Count != 0)
            {
                PowerServices.Clear();

            }
            // Search correponding power services to selected power source
            PowerTypes.Where(e => e.Source == Preference.PowerSource)
                .Select(e => e.Service).Distinct().ToList().ForEach(e =>
              {
                  PowerServices.Add(e);
              });
            if (!PowerServices.Contains(Preference.PowerService))
            {
                Preference.PowerService = PowerServices.First();
            }
        }

        public void UpdatePowerParameters()
        {
            if (PowerParameters.Count != 0)
            {
                PowerParameters.Clear();

            }
            // Search correponding power parameters to selected power source and service
            PowerTypes.Where(e => e.Source == Preference.PowerSource
                               && e.Service == Preference.PowerService)
                .Select(e => e.ParameterType).Distinct().OrderBy(e => (int)e).ToList().ForEach(e =>
                  {
                      PowerParameters.Add(e);
                  });
            if (!PowerParameters.Contains(Preference.PowerParameter))
            {
                Preference.PowerParameter = PowerParameters.First();
            }
        }

        public void UpdatePowerType()
        {
            // Search correponding power parameters to selected power source and service and parameter
            Preference.PowerType = PowerTypes.Where(e => e.Source == Preference.PowerSource
                               && e.Service == Preference.PowerService
                               && e.ParameterType == Preference.PowerParameter).First();
            Console.WriteLine("Selected Power type " + Preference.PowerType);
        }

        public PowerInputModel(IPowerPreference preference)
        {
            Preference = preference;
            UpdatePowerSources();
        }
    }
}
