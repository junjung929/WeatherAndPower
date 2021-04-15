using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WeatherAndPower.Contracts;

namespace WeatherAndPower.UI
{
    public class WeatherInputViewModel : InputViewModelBase
    {
        private IWeatherInputModel _model;
        public IWeatherInputModel Model
        {
            get { return _model; }
            private set
            {
                if (_model != value)
                {
                    _model = value;
                }
            }
        }

        public WeatherType.ParameterEnum SelectedParameterType { get; set; }

        private string _cityName;
        public string CityName
        {
            get { return _cityName; }
            set { _cityName = value; NotifyPropertyChanged("CityName"); }
        }

        public ObservableCollection<WeatherType.ParameterEnum> WeatherParameters { get; set; } = new ObservableCollection<WeatherType.ParameterEnum>()
        {
            WeatherType.ParameterEnum.Observation,
            WeatherType.ParameterEnum.Forecast
        };

        private WeatherType.ParameterEnum _selectedParameter { get; set; } = WeatherType.ParameterEnum.Observation;
        public WeatherType.ParameterEnum SelectedParameter
        {
            get { return _selectedParameter; }
            set { _selectedParameter = value; NotifyPropertyChanged("SelectedParameter"); }
        }

        private ObservableCollection<WeatherType> _weatherTypes { get; set; }
        public ObservableCollection<WeatherType> WeatherTypes
        {
            get { return _weatherTypes; }
            set { _weatherTypes = value; NotifyPropertyChanged("WeatherTypes"); }
        }

        private WeatherType _selectedWeatherType { get; set; }
        public WeatherType SelectedWeatherType
        {
            get { return _selectedWeatherType; }
            set { _selectedWeatherType = value; NotifyPropertyChanged("SelectedWeatherType"); }
        }
        public List<WeatherType> SelectedWeatherTypes = new List<WeatherType>();

        public ObservableCollection<WeatherType> Medians { get; set; } = new ObservableCollection<WeatherType>()
        {
            WeatherType.AveTempMedian, WeatherType.MinTempMedian, WeatherType.MaxTempMedian
        };

        private WeatherType _selectedMedian { get; set; }
        public WeatherType SelectedMedian
        {
            get { return _selectedMedian;  }
            set { _selectedMedian = value; NotifyPropertyChanged("SelectedMedian"); }
        }


        public void UpdateWeatherTypes()
        {
            WeatherTypes = new ObservableCollection<WeatherType>(Model.GetUpdatedWeatherTypes(SelectedParameter));
        }
        //public ICommand UpdateSelectedParameterCommand { get; set; }

        public void OnUpdateSelectedWeatherParameter()
        {
            Console.WriteLine("WeatherParameter " + SelectedParameter);
            UpdateWeatherTypes();
        }
        public void OnUpdateSelectedWeatherType()
        {
            Console.WriteLine("WeatherType " + SelectedWeatherType);
        }

        public void OnUpdateSelectedMedians()
        {
            Console.WriteLine("Medians " + SelectedMedian);
        }
        public ICommand UpdateWeatherParameterCommand { get; set; }
        public ICommand UpdateWeatherTypeCommand { get; set; }
        public ICommand UpdateMedianCommand { get; set; }

        public WeatherInputViewModel(IWeatherInputModel model)
        {
            _model = model;
            UpdateWeatherTypes();
            //WeatherTypes = new ObservableCollection<WeatherType>(WeatherType.GetAll<WeatherType>());
            //UpdateSelectedParameterCommand = new UpdateSelectedParameterCommand(this);

            UpdateWeatherParameterCommand = new RelayCommand(() => OnUpdateSelectedWeatherParameter());
            UpdateWeatherTypeCommand = new RelayCommand(() => OnUpdateSelectedWeatherType());
            UpdateMedianCommand = new RelayCommand(() => OnUpdateSelectedMedians());
        }
    }
}
