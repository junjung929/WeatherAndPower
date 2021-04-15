using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
	public interface ISidebarModel
	{
		ObservableCollection<IDataSeries> Data { get; }
		void ClearGraph();
		void OpenData(string path);
		void SaveData(string path, params int[] ids);
		void SaveDataImage(string path);
		void CompareData();
		void RemoveData(int id);
		void AddPowerDataToPlotAction(PowerType powerType, DateTime startTime, DateTime endTime, string plotName);
		void AddWeatherGraphAction(string cityName, string parameters, DateTime startTime, DateTime endTime, string plotName, WeatherType.ParameterEnum parameterType);
		IAddWindowModel CreateNewAddWindow();
	}
}
