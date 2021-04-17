using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WeatherAndPower.Contracts
{
    public class Power : IData
    {
        // Million watt
        public double Megawatt { get; set; }


        public double Value
        {
            get { return Megawatt; }
        }
        public double Watt
        {
            get
            {
                return Megawatt * (10 ^ 6);
            }
            set
            {
                double megawatt = value * (10 ^ -6);
                if (megawatt != Megawatt)
                {
                    Megawatt = megawatt;
                }
            }
        }

        public double Kilowatt
        {
            get
            {
                return Megawatt * (10 ^ 3);
            }
            set
            {
                double megawatt = value * (10 ^ -3);
                if (megawatt != Megawatt)
                {
                    Megawatt = megawatt;
                }
            }
        }

        public double Gigaawatt
        {
            get
            {
                return Megawatt * (10 ^ -3);
            }
            set
            {
                double megawatt = value * (10 ^ 3);
                if (megawatt != Megawatt)
                {
                    Megawatt = megawatt;
                }
            }
        }

        public Power(double value)
        {
            this.Megawatt = value;
        }

    }
}
