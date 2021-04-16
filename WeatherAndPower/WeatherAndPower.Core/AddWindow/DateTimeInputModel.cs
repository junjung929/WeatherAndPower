using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAndPower.Contracts;
using static WeatherAndPower.Contracts.IDateTimeInputModel;

namespace WeatherAndPower.Core
{
    public class DateTimeInputModel : AbstractModel, IDateTimeInputModel
    {
        public DateTimeInputModel()
        {
            DateTimeRanges.Add(new DateTimeRange("Past Year", "Past 365 days preceding today", "pyear"));
            DateTimeRanges.Add(new DateTimeRange("Past Month", "Past 30 days preceding today", "pmonth"));
            DateTimeRanges.Add(new DateTimeRange("Past Week", "Past 7 days preceding today", "pweek"));
            DateTimeRanges.Add(new DateTimeRange("Past 24h", "Past 24 hours from now", "p24h"));
            DateTimeRanges.Add(new DateTimeRange("Next 24h", "Next 24 hours from now", "n24h"));
            DateTimeRanges.Add(new DateTimeRange("Next 7d", "Next 7 days from now", "n7d"));
            DateTimeRanges.Add(new DateTimeRange("Next 30d", "Next 30 days from now", "n30d"));
            DateTimeRanges.Add(new DateTimeRange("Last Year", "Previous calendar year", "lyear"));
            DateTimeRanges.Add(new DateTimeRange("Last Month", "Previous calendar month", "lmonth"));
            DateTimeRanges.Add(new DateTimeRange("Yesterday", null, "yesterday"));
            DateTimeRanges.Add(new DateTimeRange("Today", null, "today"));
            DateTimeRanges.Add(new DateTimeRange("Tomorrow", null, "tomorrow"));
            DateTimeRanges.Add(new DateTimeRange("This Month", null, "tmonth"));
            DateTimeRanges.Add(new DateTimeRange("This Year", null, "tyear"));
        }

        public ObservableCollection<IDateTimeRange> DateTimeRanges { get; set; } = new ObservableCollection<IDateTimeRange>();

        public DateTime DefaultDateTimeMin { get; } = DateTime.Today.AddYears(-2);
        public DateTime DefaultDateTimeMax { get; } = DateTime.Today.AddMonths(2).AddTicks(-1);

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

        private DateTime _DateTimeMin { get; set; }
        public DateTime DateTimeMin
        {
            get { return _DateTimeMin; }
            set
            {
                _DateTimeMin = value;
                NotifyPropertyChanged("DateTimeMin");
            }
        }

        private DateTime _DateTimeMax { get; set; }
        public DateTime DateTimeMax
        {
            get { return _DateTimeMax; }
            set
            {
                _DateTimeMax = value;
                NotifyPropertyChanged("DateTimeMax");
            }
        }

        public void EnableDateTimeRange(IDateTimeRange dateTimeRange, bool isEnabled)
        {
            dateTimeRange.IsEnabled = isEnabled;
        }

        public Tuple<DateTime, DateTime> GetNewDateTimeRange(IDateTimeRange dateTimeRange)
        {
            string dateTimeRangeString = dateTimeRange.Value;
            Console.WriteLine("range: " + dateTimeRangeString);
            DateTime now = DateTime.Now;
            DateTime today = DateTime.Now.Date;
            DateTime startTime = now;
            DateTime endTime = now;
            if (dateTimeRangeString == "pyear")
            {
                startTime = today.AddDays(-365);
                endTime = today.AddTicks(-1);
            }
            else if (dateTimeRangeString == "pmonth")
            {
                startTime = today.AddDays(-30);
                endTime = today.AddTicks(-1);
            }
            else if (dateTimeRangeString == "pweek")
            {
                startTime = today.AddDays(-7);
                endTime = today.AddTicks(-1);
            }
            else if (dateTimeRangeString == "p24h")
            {
                startTime = today.AddHours(now.Hour - 24);
                endTime = today.AddHours(now.Hour).AddTicks(-1);
            }
            else if (dateTimeRangeString == "n24h")
            {
                startTime = today.AddHours(now.Hour + 1);
                endTime = startTime.AddHours(24).AddTicks(-1);
            }
            else if (dateTimeRangeString == "n36h")
            {
                startTime = today.AddHours(now.Hour + 1);
                endTime = startTime.AddHours(36).AddTicks(-1);
            }
            else if (dateTimeRangeString == "n7d")
            {
                startTime = today.AddDays(1);
                endTime = startTime.AddDays(7).AddTicks(-1);
            }
            else if (dateTimeRangeString == "n30d")
            {
                startTime = today.AddDays(1);
                endTime = startTime.AddDays(30).AddTicks(-1);
            }
            else if (dateTimeRangeString == "lyear")
            {
                startTime = new DateTime(today.Year - 1, 1, 1);
                endTime = startTime.AddYears(1).AddTicks(-1);
            }
            else if (dateTimeRangeString == "lmonth")
            {
                DateTime lastMonth = today.AddMonths(-1);
                startTime = new DateTime(lastMonth.Year, lastMonth.Month, 1);
                endTime = startTime.AddMonths(1).AddTicks(-1);
            }
            else if (dateTimeRangeString == "yesterday")
            {
                startTime = today.AddDays(-1);
                endTime = startTime.AddDays(1).AddTicks(-1);
            }
            else if (dateTimeRangeString == "today")
            {
                startTime = today;
                endTime = startTime.AddDays(1).AddTicks(-1);
            }
            else if (dateTimeRangeString == "tomorrow")
            {
                startTime = today.AddDays(1);
                endTime = startTime.AddDays(1).AddTicks(-1);
            }
            else if (dateTimeRangeString == "tmonth")
            {
                startTime = new DateTime(today.Year, today.Month, 1);
                endTime = startTime.AddMonths(1).AddTicks(-1);
            }
            else if (dateTimeRangeString == "tyear")
            {
                startTime = new DateTime(today.Year, 1, 1);
                endTime = startTime.AddYears(1).AddTicks(-1);
            }

            return new Tuple<DateTime, DateTime>(startTime, endTime);
        }

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

        public void UpdateDateTimes(IDateTimeRange dateTimeRange)
        {
            Console.WriteLine("Selected datetime button " + dateTimeRange);
            var (startTime, endTime) = GetNewDateTimeRange(dateTimeRange);
            Console.WriteLine("StartTime " + startTime);
            Console.WriteLine("endTime " + endTime);
            StartTime = AdjustDateTime(startTime);
            EndTime = AdjustDateTime(endTime);
        }
    }
}
