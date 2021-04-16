using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherAndPower.Contracts;
using static WeatherAndPower.Contracts.IDateTimeInputModel;

namespace WeatherAndPower.UI
{
    public class DateTimeViewModel : ViewModelBase
    {
        #region Properties
        private IDateTimeInputModel _model;
        public IDateTimeInputModel Model
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
        public ObservableCollection<IDateTimeRange> DateTimeRanges
        {
            get
            {
                return Model.DateTimeRanges;
            }
        }

        private IDateTimeRange _SelectedDateTimeRange { get; set; }
        public IDateTimeRange SelectedDateTimeRange
        {
            get { return _SelectedDateTimeRange; }
            set
            {
                _SelectedDateTimeRange = value; 
                NotifyPropertyChanged("SelectedDateTimeRange");
            }
        }


        public DateTime StartTime
        {
            get { return Model.StartTime; }
            set
            {
                Model.StartTime = value;
                NotifyPropertyChanged("StartTime");
            }
        }

        public DateTime EndTime
        {
            get { return Model.EndTime; }
            set
            {
                Model.EndTime = value;
                NotifyPropertyChanged("EndTime");
            }
        }
        public DateTime DefaultDateTimeMin { get { return Model.DefaultDateTimeMin; } }
        public DateTime DefaultDateTimeMax { get { return Model.DefaultDateTimeMax; } }

        public DateTime DateTimeMin
        {
            get { return Model.DateTimeMin; }
            set
            {
                Model.DateTimeMin = value;
                NotifyPropertyChanged("DateTimeMin");
            }
        }
        public DateTime DateTimeMax
        {
            get { return Model.DateTimeMax; }
            set
            {
                Model.DateTimeMax = value;
                NotifyPropertyChanged("DateTimeMax");
            }
        }
        #endregion

        //public RelayCommand UpdateDateTimeCommand => new RelayCommand(()
        //    => Model.UpdateDateTimes(SelectedDateTimeRange));
        public DateTimeViewModel(IDateTimeInputModel model)
        {
            _model = model;
            //model.UpdateDateTimeMinMaxToDefault();
        }
    }
}
