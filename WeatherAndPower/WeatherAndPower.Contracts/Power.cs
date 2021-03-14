using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WeatherAndPower.Contracts
{
    public class Power : IData
    {
        public enum PowerTypes
        {
            EConsum = 124,  // Electricity consumption in Finland
            EConsumForecast24H = 165, // Electricity consumption forecast for the next 24 hours
            EProdPrediction24H = 242, // A tentative production prediction for the next 24 hours as hourly energy
            EProd = 74, // Electricity production in Finland
            WindPowerGeneration = 75, // Wind power generation - hourly data
            WindPowerProdRT = 181, // Wind power production - real time data
            NucPowerProdRT = 188, // Nuclear power production - real time data
            HydroPowerProdRT = 191, // Hydro power production - real time data

        }
        public static Dictionary<PowerTypes, string> powerTypes = new Dictionary<PowerTypes, string>()
        {
            { PowerTypes.EConsum, "Electricity consumption in Finland" },
            { PowerTypes.EConsumForecast24H, "Electricity consumption forecast for the next 24 hours" },
            { PowerTypes.EProdPrediction24H, "A tentative production prediction for the next 24 hours as hourly energy" },
            { PowerTypes.EProd, "Electricity production in Finland" },
            { PowerTypes.WindPowerGeneration, "Wind power generation - hourly data" },
            { PowerTypes.WindPowerProdRT, "Wind power production - real time data" },
            { PowerTypes.NucPowerProdRT, "Nuclear power production - real time data" },
            { PowerTypes.HydroPowerProdRT, "Hydro power production - real time data" },
        };

        public Power(PowerTypes variableId, double value, DateTime startTime, DateTime endTime )
        {
            this.VariableId = variableId;
            this.Value = value;
            this.StartTime = startTime;
            this.EndTime = endTime;
        }
        public PowerTypes VariableId { get; } 

        public double Value
        {
            get; 
        }
        public DateTime StartTime
        {
            get;
        }

        public DateTime EndTime
        {
            get;
        }

    }
}
