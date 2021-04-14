using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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

    public enum WindowTypes
	{
        Pie
	}

    public class Globals
    {
        public static Random rand = new Random();

        // Maximum data points to draw on graph per line
        public static int MaximumDataPoints = 200;

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

		public static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
		{
			WriteIndented = true,
		};

        public static Type GetTypeFromDataFormat(DataFormat format)
		{
            switch(format) {
                case DataFormat.Temperature:
                    return typeof(Temperature);
                case DataFormat.Power:
                    return typeof(Power);
                case DataFormat.Cloudiness:
                    return typeof(WeatherType);

                default:
                    return typeof(IData);
			}
		}

        public static DataFormat GetDataFormatOfData(IData data)
		{
            if (data is Temperature) {
                return DataFormat.Temperature;
			} else if (data is Power) {
                return DataFormat.Power;
			} else {
                throw new Exception("Unrecognized dataformat");
			}
		}

        public static IData GetIDataFromDataFormat(DataFormat format, double value)
		{
            switch(format) {
                case DataFormat.Temperature:
                    return new Temperature(value);
                case DataFormat.Power:
                    return new Power(value);
                default:
                    throw new Exception("Unrecognized DataFormat");
			}
		}
    }
}
