using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAndPower.Contracts;
using WeatherAndPower.Core;
using static WeatherAndPower.Contracts.IWeatherInputModel;

namespace WeatherAndPower.Core
{
    class WeatherPreference : Preference, IWeatherPreference
    {
        public WeatherType.ParameterEnum _WeatherParameter { get; set; } = WeatherType.ParameterEnum.Observation;
        public WeatherType.ParameterEnum WeatherParameter
        {
            get { return _WeatherParameter; }
            set
            {
                _WeatherParameter = value;
                NotifyPropertyChanged("WeatherParameter");
            }
        }

        public ObservableCollection<WeatherType> WeatherTypes { get; set; } = new ObservableCollection<WeatherType>();
        public ECity _CityName { get; set; }
        public ECity CityName
        {
            get { return _CityName; }
            set
            {
                _CityName = value;
                NotifyPropertyChanged("WeatherParameter");
            }
        }
    }
}
