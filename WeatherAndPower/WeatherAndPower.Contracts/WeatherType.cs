using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
    public class WeatherType : Enumeration
    {
        public enum ParameterEnum
        {
            Observation = 0x01,
            Forecast = 0x02,
            Median = 0x04
        }
        // Observation Enum definitions
        public static WeatherType TempObserv = new WeatherType(1, "t2m")
        {
            Description = "Air Temperature",
            Format = DataFormat.Temperature,
            ParameterType = ParameterEnum.Observation
        };
        public static WeatherType WindObserv = new WeatherType(2, "ws_10min")
        {
            Description = "Wind speed",
            Format = DataFormat.Wind,
            ParameterType = ParameterEnum.Observation
        };
        public static WeatherType HumidObserv = new WeatherType(3, "rh")
        {
            Description = "Relative humidity",
            Format = DataFormat.Humidity,
            ParameterType = ParameterEnum.Observation
        };
        public static WeatherType PrecipitationObserv = new WeatherType(4, "r_1h")
        {
            Description = "Precipitation amount",
            Format = DataFormat.Precipitation,
            ParameterType = ParameterEnum.Observation
        };
        public static WeatherType CloundObserv = new WeatherType(5, "n_man")
        {
            Description = "Cloud amount",
            Format = DataFormat.Cloudiness,
            ParameterType = ParameterEnum.Observation
        };

        // Forecast enum definitions
        public static WeatherType TempForecast = new WeatherType(6, "Temperature")
        {
            Description = "Air Temperature",
            Format = DataFormat.Temperature,
            ParameterType = ParameterEnum.Forecast
        };
        public static WeatherType WindForecast = new WeatherType(7, "WindSpeedMS")
        {
            Description = "Wind Speed",
            Format = DataFormat.Wind,
            ParameterType = ParameterEnum.Forecast
        };
        public static WeatherType HumidForecast = new WeatherType(8, "Humidity")
        {
            Description = "Relative humidity",
            Format = DataFormat.Humidity,
            ParameterType = ParameterEnum.Forecast
        };
        public static WeatherType Precipitation1hForecast = new WeatherType(9, "Precipitation1h")
        {
            Description = "Houlry Precipitation",
            Format = DataFormat.Precipitation,
            ParameterType = ParameterEnum.Forecast
        };
        public static WeatherType PrecipitationAmountForecast = new WeatherType(10, "PrecipitationAmount")
        {
            Description = "Precipitation Amount",
            Format = DataFormat.Precipitation,
            ParameterType = ParameterEnum.Forecast
        };
        public static WeatherType TotalCloudCoverForecast = new WeatherType(11, "TotalCloudCover")
        {
            Description = "Cloudiness",
            Format = DataFormat.Cloudiness,
            ParameterType = ParameterEnum.Forecast
        };

        // median enum definitions
        public static WeatherType AveTempMedian = new WeatherType(12, "TA_PT1H_AVG")
        {
            Description = "Average temperature",
            //Format = DataFormat.Temperature,
            ParameterType = ParameterEnum.Median
        };
        public static WeatherType MaxTempMedian = new WeatherType(13, "TA_PT1H_MAX")
        {
            Description = "Max temperature",
            //Format = DataFormat.Temperature,
            ParameterType = ParameterEnum.Median
        };
        public static WeatherType MinTempMedian = new WeatherType(14, "TA_PT1H_MIN")
        {
            Description = "Min temperature",
            //Format = DataFormat.Temperature,
            ParameterType = ParameterEnum.Median
        };


        public DataFormat Format { get; private set; }

        public string Description { get; private set; }

        public ParameterEnum ParameterType { get; private set; }

        // Data period. Unit: minute
        public int Interval { get; private set; }

        public WeatherType(int id, string name) : base(id, name)
        {
            Interval = 60;
        }
    }
}
