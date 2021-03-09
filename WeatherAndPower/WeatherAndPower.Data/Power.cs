using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAndPower.Contracts;


namespace WeatherAndPower.Data
{
    public class Power : IData
    {
        public Power(double value, DateTime startTime, DateTime endTime )
        {
            this.Value = value;
            this.StartTime = startTime;
            this.EndTime = endTime;
        }
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
