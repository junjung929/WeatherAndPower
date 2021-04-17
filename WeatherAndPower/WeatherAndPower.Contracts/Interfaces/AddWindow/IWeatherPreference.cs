using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WeatherAndPower.Contracts.IWeatherInputModel;

namespace WeatherAndPower.Contracts
{
    public interface IWeatherPreference : IPreference
    {
        WeatherType.ParameterEnum WeatherParameter { get; set; }
        ObservableCollection<WeatherType> WeatherTypes { get; }
        ObservableCollection<WeatherType> Medians { get; }
        ECity CityEnum { get; set; }
    }
}
