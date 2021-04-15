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
		ObservableCollection<DataSeries> Data { get; }
		void ClearGraph();
		void OpenData(string path);
		void SaveData(string path);
		void AddData();
		void CompareData();
		void RemoveData();
	}
}
