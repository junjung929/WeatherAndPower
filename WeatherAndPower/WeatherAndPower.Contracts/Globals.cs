using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
	public enum DataFormat {
		Temperature		= 0x1,
		Power			= 0x2,
		Cloudiness		= 0x4,
		Wind			= 0x8
	}

	
}
