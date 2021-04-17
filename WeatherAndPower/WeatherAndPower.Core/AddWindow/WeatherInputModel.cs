using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAndPower.Contracts;
using static WeatherAndPower.Contracts.IWeatherInputModel;

namespace WeatherAndPower.Core
{
    public class WeatherInputModel : AbstractModel, IWeatherInputModel
    {
        private List<WeatherType> _WeatherTypes { get; } = WeatherType.GetAll<WeatherType>().ToList();
        public ObservableCollection<WeatherType.ParameterEnum> WeatherParameters { get; } = new ObservableCollection<WeatherType.ParameterEnum>()
        {
            WeatherType.ParameterEnum.Observation,
            WeatherType.ParameterEnum.Forecast
        };

        public List<Interval> Intervals { get; } = new List<Interval>()
        {
            new Interval(5),
            new Interval(10),
            new Interval(30),
            new Interval(60),
            new Interval(360),
            new Interval(720),
            new Interval(1440)
            //new Interval(1440*7),
            //new Interval(1440*30)
        };
        private IWeatherPreference _Preference { get; set; }
        public IWeatherPreference Preference
        {
            get { return _Preference; }
            set
            {
                _Preference = value;
                NotifyPropertyChanged("Preference");
            }
        }

        public ObservableCollection<WeatherType> WeatherTypes { get; }
            = new ObservableCollection<WeatherType>();

        public ObservableCollection<WeatherType> Mideans { get; }
            = new ObservableCollection<WeatherType>() {
                WeatherType.AveTempMedian,
                WeatherType.MinTempMedian,
                WeatherType.MaxTempMedian,
            };

        public ObservableCollection<IWeatherInputModel.ECity> Cities { get; } =
            new ObservableCollection<ECity>(Enum.GetValues(typeof(ECity)).Cast<ECity>());

        public IDateTimeInputModel CreateDateTimeInputModel()
        {
            return new DateTimeInputModel(Preference);
        }

        public List<WeatherType> GetUpdatedWeatherTypes(WeatherType.ParameterEnum weatherParameter)
        {
            return _WeatherTypes.FindAll(weatherType =>
                weatherType.ParameterType
                == weatherParameter);
        }

        public List<Interval> GetUpdatedIntervals(int minInterval)
        {
            var intervals = new List<Interval>();
            Intervals.ForEach(interval =>
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

        public void UpdateWeatherTypes()
        {
            if (WeatherTypes.Count != 0)
            {
                WeatherTypes.Clear();
            }
            foreach (var item in _WeatherTypes.Where(weatherType =>
                weatherType.ParameterType == Preference.WeatherParameter))
            {
                WeatherTypes.Add(item);
            }

        }

        public WeatherInputModel(IWeatherPreference preference)
        {
            Preference = preference;
            UpdateWeatherTypes();
        }
    }
}
