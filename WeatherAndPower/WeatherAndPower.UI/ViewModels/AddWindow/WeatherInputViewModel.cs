using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherAndPower.Contracts;
using WeatherAndPower.UI.Commands;

namespace WeatherAndPower.UI.ViewModels.AddWindow
{
    public class WeatherInputViewModel : ViewModelBase
    {
        MainViewModel ParentViewModel { get; set; }
       
        public WeatherType.ParameterEnum SelectedParameterType { get; set; }

        private string _cityName;
        public string CityName
        {
            get { return _cityName; }
            set { _cityName = value; NotifyPropertyChanged("CityName"); }
        }

        public ObservableCollection<WeatherType> WeatherTypes { get; set; }

        public List<WeatherType> SelectedParameters { get; set; } = new List<WeatherType>();

        public ICommand UpdateSelectedParameterCommand { get; set; }

        public WeatherInputViewModel(MainViewModel viewModel)
        {
            ParentViewModel = viewModel;
            WeatherTypes = new ObservableCollection<WeatherType>(WeatherType.GetAll<WeatherType>());
            UpdateSelectedParameterCommand = new UpdateSelectedParameterCommand(this);
        }
    }
}
