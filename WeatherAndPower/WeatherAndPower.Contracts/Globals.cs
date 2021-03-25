using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Test = System.Collections.Generic.List<System.Tuple<double, double>>;

namespace WeatherAndPower.Contracts
{
    public enum DataFormat
    {
        Temperature = 0x1,
        Power = 0x2,
        Cloudiness = 0x4,
        Wind = 0x8,
        Humidity = 0x16,
        Precipitation = 0x32
    }

    public class Globals
    {
        public static Random rand = new Random();
        public static Dictionary<DataFormat, string> AxisNames
            = new Dictionary<DataFormat, string>()
            {
                { DataFormat.Temperature, "TemperatureAxis" },
                { DataFormat.Power, "PowerAxis" },
                { DataFormat.Cloudiness, "CloudinessAxis" },
                { DataFormat.Wind, "WindAxis" }
            };

        public static int MaxTimeAxisLabels = 10; // Maximum amount of labels on X-axis (time)
        public static List<TimeSpan?> TimeIntervalOptions = new List<TimeSpan?>()
            {
                new TimeSpan(0, 5, 0),
                new TimeSpan(0, 15, 0),
                new TimeSpan(0, 30, 0),
                new TimeSpan(1, 0, 0),
                new TimeSpan(2, 0, 0),
                new TimeSpan(3, 0, 0),
                new TimeSpan(6, 0, 0),
                new TimeSpan(12, 0, 0),
                new TimeSpan(1, 0, 0, 0),
                new TimeSpan(7, 0, 0, 0)
            };
    }
}
