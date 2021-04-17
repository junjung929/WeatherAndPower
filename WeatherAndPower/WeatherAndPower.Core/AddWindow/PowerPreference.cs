using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAndPower.Contracts;

namespace WeatherAndPower.Core
{
    public class PowerPreference : AbstractModel, IPowerPreference
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
        public PowerType.ParameterEnum PowerParameter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IAddWindowModel.DataTypeEnum DataType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime StartTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime EndTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string PlotName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Interval { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
