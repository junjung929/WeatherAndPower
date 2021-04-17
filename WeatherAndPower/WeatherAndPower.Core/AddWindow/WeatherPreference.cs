using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAndPower.Contracts;
using WeatherAndPower.Core;

namespace WeatherAndPower.Core
{
    class WeatherPreference : Preference, IWeatherPreference
    {
        public WeatherType.ParameterEnum _WeatherParameter { get; set; }
        public WeatherType.ParameterEnum WeatherParameter
        {
            get { return _WeatherParameter; }
            set
            {
                _WeatherParameter = value;
                NotifyPropertyChanged("WeatherParameter");
            }
        }

        public ObservableCollection<WeatherType> WeatherTypes { get; set; }
        public string _CityName { get; set; }
        public string CityName
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
