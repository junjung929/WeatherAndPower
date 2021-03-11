using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAndPower.Contracts;

namespace WeatherAndPower.Contracts
{
	public class Temperature : IData
	{
		public double Kelvin { get; set; }
		public double Celsius
		{
			get { return Kelvin - 273.15; }
			set {
				var kelvin = value + 273.15;
				if (Kelvin != kelvin) {
					Kelvin = kelvin;
				}
			}
		}
		public double Fahrenheit
		{
			get { return (Kelvin * (9.0/5.0)) + 459.67; }
			set {
				var kelvin = (value + 273) * (5.0 / 9.0);
				if (Kelvin != kelvin) {
					Kelvin = kelvin;
				}
			}
		}

		public double Value
		{
			get {
				// Could query some setting here to see which value to return
				return Celsius;
			}
		}

		public Temperature(double value)
		{
			Celsius = value; // Celsius is the default type
		}
	}
}
