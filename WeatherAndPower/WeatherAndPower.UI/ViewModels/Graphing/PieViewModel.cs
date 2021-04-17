using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAndPower.Contracts;

namespace WeatherAndPower.UI
{
	public class PieViewModel : ViewModelBase
	{
		private IPieModel Model { get; set; }

		public string Name
		{
			get {
				return Model.Name;
			}
		}


		public ObservableCollection<DataPoint> Data
		{
			get { return Model.Data; }
		}

		public PieViewModel(IPieModel model)
		{
			Model = model;
			NotifyPropertyChanged("Data");
		}
	}
}
