using System;
using System.Collections.Generic;
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
        public List<DateTimeRange> DateTimeRanges { get; set; }
        private DateTimeRange _dateTimeRange { get; set; }
        public DateTimeRange DateTimeRange
        {
            get { return _dateTimeRange; }
            set { _dateTimeRange = value;
                NotifyPropertyChanged("DateTimeRange"); }
        }

        private bool _isStartTimePickerEnabled = true;
        private bool _isEndTimePickerEnabled = true;
        public bool IsStartTimePickerEnabled
        {
            get { return _isStartTimePickerEnabled; }
            set
            {
                _isStartTimePickerEnabled = value;
                NotifyPropertyChanged(nameof(IsStartTimePickerEnabled));
            }
        }
        public bool IsEndTimePickerEnabled
        {
            get { return _isEndTimePickerEnabled; }
            set
            {
                _isEndTimePickerEnabled = value;
                NotifyPropertyChanged(nameof(IsEndTimePickerEnabled));
            }
        }

        private DateTime _startTime = DateTime.Now;
        private DateTime _endTime = DateTime.Now;

        public DateTime StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
                NotifyPropertyChanged("StartTime");
            }
        }

        public DateTime EndTime
        {
            get { return _endTime; }
            set
            {
                _endTime = value;
                NotifyPropertyChanged("EndTime");
            }
        }
        public DateTime DefaultDateTimeMin { get; private set; } = DateTime.Today.AddYears(-2);
        public DateTime DefaultDateTimeMax { get; private set; } = DateTime.Today.AddMonths(2).AddTicks(-1);

        private DateTime _dateTimeMin;
        private DateTime _dateTimeMax;
        public DateTime DateTimeMin
        {
            get { return _dateTimeMin; }
            set { _dateTimeMin = value; NotifyPropertyChanged("DateTimeMin"); }
        }
        public DateTime DateTimeMax
        {
            get { return _dateTimeMax; }
            set { _dateTimeMax = value; NotifyPropertyChanged("DateTimeMax"); }
        }

        public void SetDateTimeMinMaxToDefault()
        {
            DateTimeMin = DefaultDateTimeMin;
            DateTimeMax = DefaultDateTimeMax;
        }


        /// <summary>
        /// Check whether datetime is valid
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>
        /// Returns -1 if datetime is smaller than minimum range
        /// Returns 0 if datetime is valid
        /// Returns 1 if datetime is bigger than minimum range
        /// </returns>
        public int CheckDateTimeValid(DateTime dateTime)
        {
            if (DateTime.Compare(dateTime, DateTimeMin) < 0)
            {
                System.Windows.MessageBox.Show("The minimum range of date time is "
                    + DateTimeMin.ToString("ddd dd'/'MM'/'yyyy HH:mm:ss", new CultureInfo("en-US")));
                return -1;
            }
            else if (DateTime.Compare(dateTime, DateTimeMax) > 0)
            {
                System.Windows.MessageBox.Show("The maximum range of date time is "
                    + DateTimeMax.ToString("ddd dd'/'MM'/'yyyy HH:mm:ss", new CultureInfo("en-US")));
                return 1;
            }
            return 0;
        }

        public void UpdateDateTimeMinMax(DateTime min, DateTime max)
        {
            var startTime = StartTime;
            if (DateTime.Compare(startTime, min) < 0)
            {
                startTime = min;
            }
            else if (DateTime.Compare(startTime, max) > 0)
            {
                startTime = max;
            }
            StartTime = startTime;

            var endTime = EndTime;
            if (DateTime.Compare(endTime, min) < 0)
            {
                endTime = min;
            }
            else if (DateTime.Compare(endTime, max) > 0)
            {
                endTime = max;
            }
            EndTime = endTime;
            if (min != DateTimeMin) DateTimeMin = min;
            if (max != DateTimeMax) DateTimeMax = max;
        }

        public RelayCommand UpdateDateTimeCommand => new RelayCommand(() =>
        {
            var (startTime, endTime) = Model.GetNewDateTimeRange(DateTimeRange);
            if (IsStartTimePickerEnabled)
            {
                var isValid = CheckDateTimeValid(startTime);
                if (isValid < 0)
                {
                    startTime = DateTimeMin;
                }
                else if (isValid > 0)
                {
                    startTime = DateTimeMax;
                }
                StartTime = startTime;
            }
            if (IsEndTimePickerEnabled)
            {
                var isValid = CheckDateTimeValid(endTime);
                if (isValid < 0)
                {
                    endTime = DateTimeMin;
                }
                else if (isValid > 0)
                {
                    endTime = DateTimeMax;
                }
                EndTime = endTime;
            }
        });
        public DateTimeViewModel(IDateTimeInputModel model)
        {
            _model = model;
            DateTimeRanges = _model.DateTimeRanges;
            SetDateTimeMinMaxToDefault();
        }
    }
}
