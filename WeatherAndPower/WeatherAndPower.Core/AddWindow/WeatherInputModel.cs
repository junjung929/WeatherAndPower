using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAndPower.Contracts;

namespace WeatherAndPower.Core
{
    public class WeatherInputModel : AbstractModel, IWeatherInputModel
    {
        public List<WeatherType> WeatherTypes { get; } = WeatherType.GetAll<WeatherType>().ToList();

        public List<WeatherType> GetUpdatedWeatherTypes(WeatherType.ParameterEnum weatherParameter)
        {
            return WeatherTypes.FindAll(weatherType =>
                weatherType.ParameterType
                == weatherParameter);
        }
    }
}
