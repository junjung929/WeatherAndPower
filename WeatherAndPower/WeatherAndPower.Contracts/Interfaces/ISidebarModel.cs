using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
	public interface ISidebarModel : INotifyPropertyChanged
	{
		ObservableCollection<IDataSeries> Data { get; }
		void ClearGraph();
		void OpenData(string path);
		void SaveData(string path, params int[] ids);
		void SaveSelectedData(string path);
		void SaveDataImage(string path);
		void CompareData();
		void AddData();
		void RemoveSelectedData();
		IAddWindowModel CreateNewAddWindow();
	}
}
