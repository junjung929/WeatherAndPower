using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
    public enum DataFormat
    {
        Temperature = 0x1,
        Power = 0x2,
        Cloudiness = 0x4,
        Wind = 0x8,
        Humidity = 0x10,
        Precipitation = 0x20,
        AvgTemp = 0x40,
        MaxTemp = 0x80,
        MinTemp = 0x100
    }

    public struct Interval
    {
        public int Value { get; set; }

        public bool IsEnabled { get; set; }
        public override string ToString()
        {
            var days = Value / (60 * 24);
            var hours = Value % (60 * 24) / 60;
            var mins = Value % (60 * 24) % 60;

            return (days > 0 ? days + " days" : "")
                + (hours > 0 ? hours + " hours" : "")
                + (mins > 0 ? mins + " mins" : "");
        }
        public Interval(int Value)
        {
            this.Value = Value;
            IsEnabled = true;
        }
    }

    public enum WindowTypes
	{
        Pie
	}

    public struct TypeFormat
    {
        public DataFormat Format;
        public Type Datatype;

        public TypeFormat(DataFormat form, Type type)
        {
            Format = form;
            Datatype = type;
        }
    };


    public class Globals
    {
        public static TypeFormat TempStruct = new TypeFormat(DataFormat.Temperature, typeof(Temperature));
        public static TypeFormat WindStruct = new TypeFormat(DataFormat.Wind, typeof(Wind));
        public static TypeFormat HumidityStruct = new TypeFormat(DataFormat.Humidity, typeof(Humidity));
        public static TypeFormat CloudinessStruct = new TypeFormat(DataFormat.Cloudiness, typeof(Cloudiness));
        public static TypeFormat PrecipitationStruct = new TypeFormat(DataFormat.Precipitation, typeof(Precipitation));

        public static List<TypeFormat> pair_structs = new List<TypeFormat> { TempStruct, WindStruct, HumidityStruct, CloudinessStruct, PrecipitationStruct};


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
            foreach (var pair in pair_structs)
            {
                if (pair.Format == format)
                {
                    return pair.Datatype;
                }
            }
            if (format == DataFormat.Power)
            {
                return typeof(Power);
            }
            else
            {
                return typeof(IData);
            }

        }


        public static DataFormat GetDataFormatOfData(IData data)
        {
            // making this dynamic introduces too many problems and is not recommended
            // and I don't want to spend too much time on this :)

            if (data is Temperature)
            {
                return DataFormat.Temperature;
            }
            else if (data is Power)
            {
                return DataFormat.Power;
            }
            else if (data is Cloudiness)
            {
                return DataFormat.Cloudiness;
            }
            else if (data is Wind)
            {
                return DataFormat.Wind;
            }
            else if (data is Humidity)
            {
                return DataFormat.Humidity;
            }
            else if (data is Precipitation)
            {
                return DataFormat.Precipitation;
            }
            else
            {
                throw new Exception("Unrecognized dataformat");
            }
        }


        public static IData GetIDataFromDataFormat(DataFormat format, double value)
        {
            if (format == DataFormat.Power)
            {
                return new Power(value);
            }
            else
            {
                foreach (var pair in pair_structs)
                {
                    if (format == pair.Format)
                    {
                        var type = pair.Datatype;
                        dynamic data = Activator.CreateInstance(type, value);
                        return data;
                    }
                }
            }
            throw new Exception("Urecognized DataFormat!");


        }

    }
}
