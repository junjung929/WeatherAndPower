using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
    public class Humidity : IData
    {
        public Humidity(double value)
        {
            this.Value = value;
        }

        public double Value
        {
            get;
        }
    }
}

