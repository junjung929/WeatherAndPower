using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
    public class MinTemp : IData
    {
        public MinTemp(double value)
        {
            this.Value = value;
        }

        public double Value
        {
            get;
        }
    }
}
