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
        public IDateTimeInputModel CreateDateTimeInputModel()
        {
            return new DateTimeInputModel();
        }

        public List<WeatherType> GetUpdatedWeatherTypes(WeatherType.ParameterEnum weatherParameter)
        {
            return WeatherTypes.FindAll(weatherType =>
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
    }
}
