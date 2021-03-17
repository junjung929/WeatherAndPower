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

        private DateTime _startTime = DateTime.Now;
        private DateTime _endTime = DateTime.Now;
        public DateTime StartTime
        {
            get { return _startTime;  }
            set { _startTime = value; NotifyPropertyChanged("StartTime"); }
        }
        
        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; NotifyPropertyChanged("EndTime"); }
        }
        public string PlotName { get; set; }

        public PowerInputViewModel()
        {
            
            PowerTypes = new ObservableCollection<PowerType>(PowerType.GetAll<PowerType>());
        }
    }
}
