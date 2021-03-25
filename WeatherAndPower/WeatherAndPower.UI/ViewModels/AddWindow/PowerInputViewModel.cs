using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAndPower.Contracts;

namespace WeatherAndPower.UI.ViewModels.AddWindow
{
    public class PowerInputViewModel : ViewModelBase
    {

        public ObservableCollection<PowerType> PowerTypes { get; set; }
        public PowerType SPowerType { get; set; }

        public PowerInputViewModel()
        {
            PowerTypes = new ObservableCollection<PowerType>(PowerType.GetAll<PowerType>());
        }
    }
}
