using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
    public interface IWeatherInputModel
    {
        enum ECity
        {
            Helsinki = 0x01,
            Tampere = 0x02,
            Oulu = 0x04,
            Rovaniemi = 0x08,
            Joensuu = 0x10,
            Turku = 0x20
        }

        List<WeatherType> WeatherTypes { get; }
        List<Interval> Intervals { get; }

        List<WeatherType> GetUpdatedWeatherTypes(WeatherType.ParameterEnum weatherParameter);
        List<Interval> GetUpdatedIntervals(int minInterval);

        IDateTimeInputModel CreateDateTimeInputModel();
    }
}
