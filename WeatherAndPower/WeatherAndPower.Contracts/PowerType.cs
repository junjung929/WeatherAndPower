using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
    public class PowerType : Enumeration
    {
        public static PowerType Econsum = new PowerType(124, "Electricity consumption in Finland", "MW", 60);
        public static PowerType EConsumForecast24H = new PowerType(165, "Electricity consumption forecast for the next 24 hours", "MW", 60);
        public static PowerType EProdPrediction24H = new PowerType(242, "A tentative production prediction for the next 24 hours as hourly energy", "MW", 60);
        public static PowerType EProd = new PowerType(74, "Electricity production in Finland", "MW", 60);
        public static PowerType EProdRT = new PowerType(192, "Electricity production in Finland - real time data", "MW", 3);
        public static PowerType WindPowerGeneration = new PowerType(75, "Wind power generation - hourly data", "MW", 60);
        public static PowerType WindPowerGenerationForecast = new PowerType(245, "Wind power generation forecast - updated hourly", "MW", 60);
        public static PowerType WindPowerProdRT = new PowerType(181, "Wind power production - real time data", "MW", 3);
        public static PowerType NucPowerPordRT = new PowerType(188, "Nuclear power production - real time data", "MW", 3);
        public static PowerType HydroPowerProdRT = new PowerType(191, "Hydro power production - real time data", "MW", 3);

        public string Unit { get; private set; }
        
        // Data period. Unit: minute
        public int Interval { get; private set; }

        public PowerType(int id, string name, string unit, int interval) : base(id, name)
        {
            Unit = unit;
            Interval = interval;
        }
    }


}
