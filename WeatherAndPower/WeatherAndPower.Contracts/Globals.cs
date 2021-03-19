using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Test = System.Collections.Generic.List<System.Tuple<double, double>>;

namespace WeatherAndPower.Contracts
{
	public enum DataFormat {
		Temperature		= 0x1,
		Power			= 0x2,
		Cloudiness		= 0x4,
		Wind			= 0x8
	}

	
	public class Globals
	{
		public static Random rand = new Random();
		public static Dictionary<DataFormat, string> AxisNames
			= new Dictionary<DataFormat, string>()
			{
				{ DataFormat.Temperature, "TemperatureAxis" },
				{ DataFormat.Power, "PowerAxis" },
				{ DataFormat.Cloudiness, "CloudinessAxis" },
				{ DataFormat.Wind, "WindAxis" }
			};
	}
}
