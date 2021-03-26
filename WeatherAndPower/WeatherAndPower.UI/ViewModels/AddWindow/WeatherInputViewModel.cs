using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAndPower.Contracts;

namespace WeatherAndPower.UI.ViewModels.AddWindow
{
    class WeatherInputViewModel : ViewModelBase
    {
        public ObservableCollection<WeatherType> WeatherTypes { get; set; }
        public List<WeatherType> SWeatherTypes { get; set; } = new List<WeatherType>();

        public WeatherInputViewModel()
        {
            WeatherTypes = new ObservableCollection<WeatherType>(WeatherType.GetAll<WeatherType>());
        }
    }
}
