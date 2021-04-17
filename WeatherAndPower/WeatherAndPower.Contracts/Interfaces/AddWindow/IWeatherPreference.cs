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
        ObservableCollection<WeatherType> WeatherTypes { get; set; }
        ECity CityName { get; set; }
    }
}
