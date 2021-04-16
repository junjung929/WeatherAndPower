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
            get { return Model.DateTimeRanges; }
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

        public static DateTime DefaultDateTimeMin { get; } = DateTime.Today.AddYears(-2);
        public static DateTime DefaultDateTimeMax { get; } = DateTime.Today.AddYears(-2);

        private DateTime _startTime = DateTime.Now;
        public DateTime StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
                NotifyPropertyChanged("StartTime");
            }
        }

        private DateTime _endTime = DateTime.Now;

        public DateTime EndTime
        {
            get { return _endTime; }
            set
            {
                _endTime = value;
                NotifyPropertyChanged("EndTime");
            }
        }

        private DateTime _DateTimeMin { get; set; } = DefaultDateTimeMin;
        public DateTime DateTimeMin
        {
            get { return _DateTimeMin; }
            set
            {
                _DateTimeMin = value;
                NotifyPropertyChanged("DateTimeMin");
            }
        }

        private DateTime _DateTimeMax { get; set; } = DefaultDateTimeMax;
        public DateTime DateTimeMax
        {
            get { return _DateTimeMax; }
            set
            {
                _DateTimeMax = value;
                NotifyPropertyChanged("DateTimeMax");
            }
        }

        #endregion

        public void UpdateDateTimeMinMaxToDefault()
        {
            DateTimeMin = DefaultDateTimeMin;
            DateTimeMax = DefaultDateTimeMax;
        }

        public void UpdateDateTimeMinMax(DateTime min, DateTime max)
        {
            // Check if startTime and endtime areout of range between min and max
            // then adjust value
            // This must be done before updating minumum and maximum datetime to avoid error
            StartTime = AdjustDateTime(StartTime, min, max);
            EndTime = AdjustDateTime(EndTime, min, max);

            // Then update min and max datetime
            DateTimeMin = min;
            DateTimeMax = max;
        }

        public void UpdateDateTimes(IDateTimeRange dateTimeRange)
        {
            var (startTime, endTime) = GetNewDateTimeRange(dateTimeRange);
            StartTime = AdjustDateTime(startTime);
            EndTime = AdjustDateTime(endTime);
        }

        private Tuple<DateTime, DateTime> GetNewDateTimeRange(IDateTimeRange dateTimeRange)
        {
            string dateTimeRangeId = dateTimeRange.Id;

            DateTime now = DateTime.Now;
            DateTime today = DateTime.Now.Date;
            DateTime startTime = now;
            DateTime endTime = now;

            if (dateTimeRangeId == "pyear")
            {
                startTime = today.AddDays(-365);
                endTime = today.AddTicks(-1);
            }
            else if (dateTimeRangeId == "pmonth")
            {
                startTime = today.AddDays(-30);
                endTime = today.AddTicks(-1);
            }
            else if (dateTimeRangeId == "pweek")
            {
                startTime = today.AddDays(-7);
                endTime = today.AddTicks(-1);
            }
            else if (dateTimeRangeId == "p24h")
            {
                startTime = today.AddHours(now.Hour - 24);
                endTime = today.AddHours(now.Hour).AddTicks(-1);
            }
            else if (dateTimeRangeId == "n24h")
            {
                startTime = today.AddHours(now.Hour + 1);
                endTime = startTime.AddHours(24).AddTicks(-1);
            }
            else if (dateTimeRangeId == "n36h")
            {
                startTime = today.AddHours(now.Hour + 1);
                endTime = startTime.AddHours(36).AddTicks(-1);
            }
            else if (dateTimeRangeId == "n7d")
            {
                startTime = today.AddDays(1);
                endTime = startTime.AddDays(7).AddTicks(-1);
            }
            else if (dateTimeRangeId == "n30d")
            {
                startTime = today.AddDays(1);
                endTime = startTime.AddDays(30).AddTicks(-1);
            }
            else if (dateTimeRangeId == "lyear")
            {
                startTime = new DateTime(today.Year - 1, 1, 1);
                endTime = startTime.AddYears(1).AddTicks(-1);
            }
            else if (dateTimeRangeId == "lmonth")
            {
                DateTime lastMonth = today.AddMonths(-1);
                startTime = new DateTime(lastMonth.Year, lastMonth.Month, 1);
                endTime = startTime.AddMonths(1).AddTicks(-1);
            }
            else if (dateTimeRangeId == "yesterday")
            {
                startTime = today.AddDays(-1);
                endTime = startTime.AddDays(1).AddTicks(-1);
            }
            else if (dateTimeRangeId == "today")
            {
                startTime = today;
                endTime = startTime.AddDays(1).AddTicks(-1);
            }
            else if (dateTimeRangeId == "tomorrow")
            {
                startTime = today.AddDays(1);
                endTime = startTime.AddDays(1).AddTicks(-1);
            }
            else if (dateTimeRangeId == "tmonth")
            {
                startTime = new DateTime(today.Year, today.Month, 1);
                endTime = startTime.AddMonths(1).AddTicks(-1);
            }
            else if (dateTimeRangeId == "tyear")
            {
                startTime = new DateTime(today.Year, 1, 1);
                endTime = startTime.AddYears(1).AddTicks(-1);
            }

            return new Tuple<DateTime, DateTime>(startTime, endTime);
        }

        private DateTime AdjustDateTime(DateTime dateTime)
        {
            return AdjustDateTime(dateTime, DateTimeMin, DateTimeMax);
        }
        private DateTime AdjustDateTime(DateTime dateTime, DateTime min, DateTime max)
        {
            if (dateTime.CompareTo(min) < 0)
            {
                return min;
            }
            else if (dateTime.CompareTo(max) > 0)
            {
                return max;
            }
            return dateTime;
        }

        //public RelayCommand UpdateDateTimeCommand => new RelayCommand(()
        //    => Model.UpdateDateTimes(SelectedDateTimeRange));
        public DateTimeViewModel(IDateTimeInputModel model)
        {
            _model = model;
        }
    }
}
