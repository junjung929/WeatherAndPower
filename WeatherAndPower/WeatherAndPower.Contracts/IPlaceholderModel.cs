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

		void PlaceholderAction5();

		bool SaveChartImage(string path);

		bool SaveChart(string path, params int[] ids);

		bool LoadChart(string path);

		IAddWindowModel CreateNewAddWindow();


		void AddPowerDataToPlotAction(PowerType powerType, DateTime startTime, DateTime endTime, string plotName);

		void AddWeatherGraphAction(string cityName, string parameters, DateTime startTime, DateTime endTime, string plotName, WeatherType.ParameterEnum parameterType);
	}
}
