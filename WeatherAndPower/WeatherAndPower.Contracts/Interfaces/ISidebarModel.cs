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
		/**
		 * Main container for IDataSeries
		 */
		ObservableCollection<IDataSeries> Data { get; }

		/**
		 * Clear all plots from the graph
		 */
		void ClearGraph();

		/**
		 * Open data from JSON
		 */
		void OpenData(string path);

		/**
		 * Save all data to JSON
		 */
		void SaveData(string path, params int[] ids);

		/**
		 * Save selected data to JSON
		 */
		void SaveSelectedData(string path);

		/**
		 * Save graph as an image
		 */
		void SaveDataImage(string path);

		/**
		 * Create a data comparison
		 */
		void CompareData();

		/**
		 * add data to graph
		 */
		void AddData();

		/**
		 * Remove selected data from graph
		 */
		void RemoveSelectedData();
	}
}
