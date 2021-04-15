using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
    public interface IWeatherInputModel
    {
        List<WeatherType> WeatherTypes { get; }
        List<WeatherType> GetUpdatedWeatherTypes(WeatherType.ParameterEnum weatherParameter);
    }
}
