using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAndPower.Contracts;
using WeatherAndPower.UI.Views;

namespace WeatherAndPower.UI
{
    public class SidebarViewModel : ViewModelBase
    {
        public ISidebarModel Model { get; private set; }

        public ObservableCollection<IDataSeries> Data { 
            get {
                return Model.Data;
            }
        }

        public IDataSeries SelectedSeries { get; set; }

        public RelayCommand ClearGraphCommand => new RelayCommand(() => Model.ClearGraph());
        public RelayCommand OpenDataCommand => new RelayCommand(() => Model.OpenData("test.json"));
        public RelayCommand SaveDataCommand => new RelayCommand(() => Model.SaveData("test.json"));
        public RelayCommand SaveDataImageCommand => new RelayCommand(() => Model.SaveDataImage("test.png"));
        public RelayCommand AddDataCommand => new RelayCommand(() =>
        {
            var addWindow = new AddWindow(this);
            addWindow.Show();
        });
        public RelayCommand CompareDataCommand => new RelayCommand(() => Model.CompareData());
        public RelayCommand RemoveDataCommand => new RelayCommand(() => Model.RemoveData(0));

        public void AddPowerGraphCommand(PowerType powerType, DateTime startTime, DateTime endTime, string plotName)
        {
            Model.AddPowerDataToPlotAction(powerType, startTime, endTime, plotName);
        }
        public void AddWeatherGraphCommand(string cityName, string parameters, DateTime startTime, DateTime endTime, string plotName, WeatherType.ParameterEnum parameterType)
        {
            Model.AddWeatherGraphAction(cityName, parameters, startTime, endTime, plotName, parameterType);
        }

        public SidebarViewModel(ISidebarModel model) {
            Model = model;
        }
	}
}
