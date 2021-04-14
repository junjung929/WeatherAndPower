using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
	public interface IWindowFactory
	{
		void CreateWindow(IPieModel model);
	}
}
