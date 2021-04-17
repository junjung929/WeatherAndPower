using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
	public interface ICustomChart
	{
		Task<DateTime> Pick();
		bool Save(string fileName);
	}
}
