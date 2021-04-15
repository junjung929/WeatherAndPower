using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
    public class Precipitation : IData
    {
        public Precipitation(double value)
        {
            this.Value = value;
        }

        public double Value
        {
            get;
        }
    }
}

