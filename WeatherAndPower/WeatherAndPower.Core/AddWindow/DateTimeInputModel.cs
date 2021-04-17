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
        public ObservableCollection<IDateTimeRange> DateTimeRanges { get; set; } = new ObservableCollection<IDateTimeRange>();
        public IPreference Preference { get; set; }

        public DateTimeInputModel(IPreference preference)
        {
            Preference = preference;
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
    }
}
