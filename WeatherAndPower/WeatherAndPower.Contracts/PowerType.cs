using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
    public class PowerType : Enumeration
    {
        public enum ParameterEnum
        {
            Observation = 0x01,
            Forecast = 0x02,
            RealTime = 0x04
        }

        public enum SourceEnum
        {
            All = 0x01,
            Wind = 0x02,
            Hydro = 0x04,
            Nuclear = 0x08
        }

        public static PowerType Econsum = new PowerType(124, "Electricity consumption")
        {
            Description = "Electricity consumption in Finland",
            ParameterType = ParameterEnum.Observation,
            Interval = 60,
            Source = SourceEnum.All
        };
        public static PowerType EConsumForecast24H = new PowerType(165, "Electricity consumption forecast for the next 24 hours")
        {
            Description = "Electricity consumption forecast for the next 24 hours",
            Source = SourceEnum.All,
            ParameterType = ParameterEnum.Forecast,
            Interval = 60
        };
        public static PowerType EProdPrediction24H = new PowerType(242, "A tentative production prediction for the next 24 hours")
        {
            Description= "A tentative production prediction for the next 24 hours as hourly energy",
            Source = SourceEnum.All,
            ParameterType = ParameterEnum.Forecast,
            Interval = 60
        };
        public static PowerType EProd = new PowerType(74, "Electricity production in Finland")
        {
            Description= "Electricity production in Finland",
            Source = SourceEnum.All,
            ParameterType = ParameterEnum.Observation,
            Interval = 60
        };
        public static PowerType EProdRT = new PowerType(192, "Electricity production in Finland")
        {
            Description= "Electricity production in Finland - real time data",
            Source = SourceEnum.All,
            ParameterType = ParameterEnum.RealTime,
            Interval = 3
        };
        public static PowerType WindPowerGeneration = new PowerType(75, "Wind power generation")
        {
            Description= "Wind power generation - hourly data",
            Source = SourceEnum.Wind,
            ParameterType = ParameterEnum.Observation,
            Interval = 60
        };
        public static PowerType WindPowerGenerationForecast = new PowerType(245, "Wind power generation forecast")
        {
            Description= "Wind power generation forecast - updated hourly",
            Source = SourceEnum.Wind,
            ParameterType = ParameterEnum.Forecast,
            Interval = 60
        };
        public static PowerType WindPowerProdRT = new PowerType(181, "Wind power production")
        {
            Description= "Wind power production - real time data",
            Source = SourceEnum.Wind,
            ParameterType = ParameterEnum.RealTime,
            Interval = 3
        };
        public static PowerType NucPowerPordRT = new PowerType(188, "Nuclear power production")
        {
            Description= "Nuclear power production - real time data",
            Source = SourceEnum.Nuclear,
            ParameterType = ParameterEnum.RealTime,
            Interval = 3
        };
        public static PowerType HydroPowerProdRT = new PowerType(191, "Hydro power production")
        {
            Description= "Hydro power production - real time data",
            Source = SourceEnum.Hydro,
            ParameterType = ParameterEnum.RealTime,
            Interval = 3
        };

        public string Unit { get; private set; }

        // Data period. Unit: minute
        public int Interval { get; private set; }

        public ParameterEnum ParameterType { get; private set; }

        public SourceEnum Source { get; private set; }

        public string Description { get; private set; }

        public PowerType(int id, string name) : base(id, name)
        {
            Unit = "MW";
        }
    }


}
