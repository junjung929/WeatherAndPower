using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAndPower.Contracts;

namespace WeatherAndPower.UI
{
	public class PlaceholderViewModel : ViewModelBase
	{
		private IPlaceholderModel _Model;
		public IPlaceholderModel Model
		{
			get { return _Model; }
			private set {
				if (_Model != value) {
					_Model = value;
				}
			}
		}

		public void PlaceholderAction()
		{
			Model.PlaceholderAction();
			NotifyPropertyChanged("PlaceholderProperty");
		}
		public RelayCommand PlaceholderCommand => new RelayCommand(() => this.PlaceholderAction());

		public int PlaceholderProperty
		{
			get { return Model.PlaceholderProperty; }
		}
		
		public PlaceholderViewModel(IPlaceholderModel model)
		{
			Model = model;
		}
	}
}
