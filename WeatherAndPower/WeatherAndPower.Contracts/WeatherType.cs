using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
    public class WeatherType : Enumeration
    {
        // Observation Enum definitions
        public static WeatherType TempObserv = new WeatherType(1, "t2m")
        {
            Description = "Air Temperature",
            Format = DataFormat.Temperature,
            ParameterType = "Observation"
        };
        public static WeatherType WindObserv = new WeatherType(2, "ws_10min")
        {
            Description = "Wind speed",
            Format = DataFormat.Wind,
            ParameterType = "Observation"
        };
        public static WeatherType HumidObserv = new WeatherType(3, "rh")
        {
            Description = "Relative humidity",
            Format = DataFormat.Humidity,
            ParameterType = "Observation"
        };
        public static WeatherType PrecipitationObserv = new WeatherType(4, "rhr_1h")
        {
            Description = "Precipitation amount",
            Format = DataFormat.Precipitation,
            ParameterType = "Observation"
        };
        public static WeatherType CloundObserv = new WeatherType(5, "n_man")
        {
            Description = "Cloud amount",
            Format = DataFormat.Cloudiness,
            ParameterType = "Observation"
        };

        // Forecast enum definitions
        public static WeatherType TempForecast = new WeatherType(6, "Temperature")
        {
            Description = "Air Temperature",
            Format = DataFormat.Temperature,
            ParameterType = "Forecast"
        };
        public static WeatherType WindForecast = new WeatherType(7, "WindSpeedMS")
        {
            Description = "Wind Speed",
            Format = DataFormat.Wind,
            ParameterType = "Forecast"
        };
        public static WeatherType HumidForecast = new WeatherType(8, "Humidity")
        {
            Description = "Relative humidity",
            Format = DataFormat.Humidity,
            ParameterType = "Forecast"
        };
        public static WeatherType Precipitation1hForecast = new WeatherType(9, "Precipitation1h")
        {
            Description = "Houlry Precipitation",
            Format = DataFormat.Precipitation,
            ParameterType = "Forecast"
        };
        public static WeatherType PrecipitationAmountForecast = new WeatherType(10, "PrecipitationAmount")
        {
            Description = "Precipitation Amount",
            Format = DataFormat.Precipitation,
            ParameterType = "Forecast"
        };
        public static WeatherType TotalCloudCoverForecast = new WeatherType(11, "TotalCloudCover")
        {
            Description = "Cloudiness",
            Format = DataFormat.Cloudiness,
            ParameterType = "Forecast"
        };

        // median enum definitions
        public static WeatherType AveTempMedian = new WeatherType(7, "TA_PT1H_AVG")
        {
            Description = "Average temperature",
            Format = DataFormat.Temperature,
            ParameterType = "Median"
        };
        public static WeatherType MaxTempMedian = new WeatherType(7, "TA_PT1H_MAX")
        {
            Description = "Max temperature",
            Format = DataFormat.Temperature,
            ParameterType = "Median"
        };
        public static WeatherType MinTempMedian = new WeatherType(7, "TA_PT1H_MIN")
        {
            Description = "Min temperature",
            Format = DataFormat.Temperature,
            ParameterType = "Median"
        };


        public DataFormat Format { get; private set; }

        public string Description { get; private set; }

        public string ParameterType { get; private set; }

        public WeatherType(int id, string name) : base(id, name)
        {
        }
    }
}
