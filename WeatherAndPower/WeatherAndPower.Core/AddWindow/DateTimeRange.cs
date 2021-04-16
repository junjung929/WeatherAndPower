using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAndPower.Contracts;

namespace WeatherAndPower.Core
{
    class DateTimeRange : AbstractModel, IDateTimeRange
    {
        private string _Name { get; set; }
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value; NotifyPropertyChanged("Name");
            }
        }

        private string _Description { get; set; }
        public string Description
        {
            get { return _Description; }
            set { _Description = value; NotifyPropertyChanged("Description"); }
        }

        private string _Value { get; set; }
        public string Value
        {
            get { return _Value; }
            set { _Value = value; NotifyPropertyChanged("Value"); }
        }

        private bool _IsEnabled { get; set; } = true;
        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set { _IsEnabled = value; NotifyPropertyChanged("IsEnabled"); }
        }

        private bool _IsSelected { get; set; } = false;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set { _IsSelected = value; NotifyPropertyChanged("IsSelected"); }
        }

        public DateTimeRange(string name, string description, string value)
        {
            Name = name;
            Description = description;
            Value = value;
        }
    }
}
