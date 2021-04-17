using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAndPower.Contracts;

namespace WeatherAndPower.Core
{
    public class Preference : AbstractModel, IPreference
    {
        private IAddWindowModel.DataTypeEnum _DataType { get; set; }
        public IAddWindowModel.DataTypeEnum DataType
        {
            get { return _DataType; }
            set
            {
                _DataType = value;
                NotifyPropertyChanged("DataType");
            }
        }
        private DateTime _StartTime { get; set; }
        public DateTime StartTime
        {
            get { return _StartTime; }
            set
            {
                _StartTime = value;
                NotifyPropertyChanged("EndTime");
            }
        }
        private DateTime _EndTime { get; set; }
        public DateTime EndTime
        {
            get { return _EndTime; }
            set
            {
                _EndTime = value;
                NotifyPropertyChanged("EndTime");
            }
        }
        private string _PlotName { get; set; }
        public string PlotName
        {
            get { return _PlotName; }
            set
            {
                _PlotName = value;
                NotifyPropertyChanged("PlotName");
            }
        }
        private int _Interval { get; set; }
        public int Interval
        {
            get { return _Interval; }
            set
            {
                _Interval = value;
                NotifyPropertyChanged("Interval");
            }
        }
    }
}
