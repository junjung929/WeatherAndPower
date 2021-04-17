using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAndPower.Contracts;

namespace WeatherAndPower.Core
{
    public class PowerPreference : Preference, IPowerPreference
    {
        private PowerType.SourceEnum _PowerSource { get; set; }
        public PowerType.SourceEnum PowerSource
        {
            get { return _PowerSource; }
            set
            {
                _PowerSource = value;
                NotifyPropertyChanged("PowerSource");
            }
        }
        private PowerType.ServiceEnum _PowerService { get; set; }
        public PowerType.ServiceEnum PowerService
        {
            get { return _PowerService; }
            set
            {
                _PowerService = value;
                NotifyPropertyChanged("PowerService");
            }
        }
        private PowerType.ParameterEnum _PowerParameter { get; set; }
        public PowerType.ParameterEnum PowerParameter
        {
            get { return _PowerParameter; }
            set
            {
                _PowerParameter = value;
                NotifyPropertyChanged("PowerParameter");
            }
        }
    }
}
