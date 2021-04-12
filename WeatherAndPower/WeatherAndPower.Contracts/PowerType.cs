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
            Nuclear = 0x08,
            Solar = 0x16
        }

        public static PowerType Econsum = new PowerType(124, "Electricity consumption in Finland")
        {
            Description = "Electricity consumption in Finland is based on Fingrid's production measurements.\n" +
            "Minor part of production which is not measured is estimated.\n" +
            "The consumption is calculated as follows: Consumption = Production + Import - Export. " +
            "Updated hourly.",
            ParameterType = ParameterEnum.Observation,
            Interval = 60,
            Source = SourceEnum.All
        };
        public static PowerType EConsumForecast24H = new PowerType(165, "Electricity consumption forecast - max. next 24 hours")
        {
            Description = "An hourly consumption forecast for the next 24 hours made by Fingrid.\n" +
            "Forecast is published previous day at 12:00 EET.",
            Source = SourceEnum.All,
            ParameterType = ParameterEnum.Forecast,
            Interval = 60
        };
        public static PowerType EProdPrediction24H = new PowerType(242, "A tentative production prediction - max. next 24 hours")
        {
            Description = "Hourly electricity generation forecast is based on the production plans " +
            "that balance responsible parties have reported to Fingrid.\n" +
            "The forecast is published daily by 6.00 pm for the next day, and it is not updated " +
            "to match the updated production plans that balance responsible parties send to Fingrid hourly.",
            Source = SourceEnum.All,
            ParameterType = ParameterEnum.Forecast,
            Interval = 60
        };
        public static PowerType EProd = new PowerType(74, "Electricity production in Finland")
        {
            Description = "Hourly electricity production in Finland are based on Fingrid's measurements.\n" +
            "Minor part of production which is not measured is estimated. " +
            "Updated hourly.",
            Source = SourceEnum.All,
            ParameterType = ParameterEnum.Observation,
            Interval = 60
        };
        public static PowerType EProdRT = new PowerType(192, "Electricity production in Finland - real time data")
        {
            Description = "Electricity production in Finland based on the real-time measurements " +
            "in Fingrid's operation control system The data is updated every 3 minutes.",
            Source = SourceEnum.All,
            ParameterType = ParameterEnum.RealTime,
            Interval = 3
        };
        public static PowerType WindPowerGeneration = new PowerType(75, "Wind power generation")
        {
            Description = "Finnish hourly wind power generation " +
            "is a sum of measurements from wind parks supplied to Fingrid " +
            "and of the estimate Fingrid makes from non-measured wind parks.\n" +
            "Non-measured wind parks are about a tenth of the production capacity.",
            Source = SourceEnum.Wind,
            ParameterType = ParameterEnum.Observation,
            Interval = 60
        };
        public static PowerType WindPowerGenerationForecast = new PowerType(245, "Wind power generation forecast - max. next 36 hours")
        {
            Description = "Finnish wind power generation forecast for the next 36 hours. Updated hourly.",
            Source = SourceEnum.Wind,
            ParameterType = ParameterEnum.Forecast,
            Interval = 60
        };
        public static PowerType WindPowerProdRT = new PowerType(181, "Wind power production - real time data")
        {
            Description = "Wind power production based on the real-time measurements in Fingrid's operation control system.\n" +
            "About a tenth of the production capacity is estimated as measurements aren't available.\n" +
            "The data is updated every 3 minutes.",
            Source = SourceEnum.Wind,
            ParameterType = ParameterEnum.RealTime,
            Interval = 3
        };
        public static PowerType NucPowerPordRT = new PowerType(188, "Nuclear power production - real time data")
        {
            Description = "Nuclear power production in Finland based on the real-time measurements in Fingrid's operation control system.\n" +
            "The data is updated every 3 minutes.",
            Source = SourceEnum.Nuclear,
            ParameterType = ParameterEnum.RealTime,
            Interval = 3
        };
        public static PowerType HydroPowerProdRT = new PowerType(191, "Hydro power production - real time data")
        {
            Description = "Hydro power production in Finland based on the real-time measurements in Fingrid's operation control system.\n" +
            "The data is updated every 3 minutes.",
            Source = SourceEnum.Hydro,
            ParameterType = ParameterEnum.RealTime,
            Interval = 3
        };
        public static PowerType SolarPowerGenerationForecast= new PowerType(248, "Solar power generation forecast - max. next 36 hours")
        {
            Description = "Hourly updated solar power generation forecast for the next 36 hours.",
            Source = SourceEnum.Solar,
            ParameterType = ParameterEnum.Forecast,
            Interval = 60
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
