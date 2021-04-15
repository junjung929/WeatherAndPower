using System;
using System.Collections.Generic;
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

        public List<DateTimeRange> DateTimeRanges { get; set; } = new List<DateTimeRange>();

        public Tuple<DateTime, DateTime> GetNewDateTimeRange(DateTimeRange dateTimeRange)
        {
            string dateTimeRangeString = dateTimeRange.Value;
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
    }
}
