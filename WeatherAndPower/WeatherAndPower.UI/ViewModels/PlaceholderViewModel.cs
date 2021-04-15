//using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherAndPower.Contracts;
using WeatherAndPower.UI.Commands;
using WeatherAndPower.UI.Views;

namespace WeatherAndPower.UI
{
    public class PlaceholderViewModel : ViewModelBase
    {
        private IPlaceholderModel _Model;
        public IPlaceholderModel Model
        {
            get { return _Model; }
            private set
            {
                if (_Model != value)
                {
                    _Model = value;
                }
            }
        }

        public string PlaceholderText
        {
            get { return Model.DataName; }
            set
            {
                if (Model.DataName != value)
                {
                    Model.DataName = value;
                    NotifyPropertyChanged("PlaceholderText");
                }
            }
        }


        public RelayCommand PlaceholderCommand1 => new RelayCommand(() => Model.PlaceholderAction1());
        public RelayCommand PlaceholderCommand2 => new RelayCommand(() => Model.PlaceholderAction2());
        public RelayCommand PlaceholderCommand3 => new RelayCommand(() => Model.PlaceholderAction3());

        public RelayCommand PlaceholderCommand5 => new RelayCommand(() => Model.PlaceholderAction5());
        public RelayCommand LoadChartCommand => new RelayCommand(() => Model.LoadChart("test.json"));

        public void AddPowerGraphCommand(PowerType powerType, DateTime startTime, DateTime endTime, string plotName)
        {
            Model.AddPowerDataToPlotAction(powerType, startTime, endTime, plotName);
        }

        public void AddWeatherGraphCommand(string cityName, string parameters, DateTime startTime, DateTime endTime, string plotName, WeatherType.ParameterEnum parameterType)
        {
            Model.AddWeatherGraphAction(cityName, parameters, startTime, endTime, plotName, parameterType);
        }

        public ICommand OpenAddWindowCommand { get; set; }

        public PlaceholderViewModel(IPlaceholderModel model)
        {
            Model = model;
            OpenAddWindowCommand = new OpenAddWindowCommand(this);
        }
    }
}
