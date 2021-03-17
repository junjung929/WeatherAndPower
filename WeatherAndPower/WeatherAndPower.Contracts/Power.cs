using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WeatherAndPower.Contracts
{
    public class Power : IData
    {
        public Power(int variableId, double value, DateTime startTime, DateTime endTime)
        {
            this.VariableId = variableId;
            this.Value = value;
            this.StartTime = startTime;
            this.EndTime = endTime;
        }
        public int VariableId { get; }

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
