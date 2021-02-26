using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
	public interface IPlaceholderModel
	{
		int PlaceholderProperty { get; }
		string DataName { get; set; }
		void PlaceholderAction1();
		void PlaceholderAction2();
		void PlaceholderAction3();
	}
}
