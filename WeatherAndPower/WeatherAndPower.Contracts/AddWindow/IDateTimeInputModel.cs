using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
    public interface IDateTimeInputModel
    {
        struct DateTimeRange
        {
            public string Name { get; set; } 
            public string Description { get; set; }
            public string Value { get; set; }

            public DateTimeRange(string name, string? description, string value)
            {
                Name = name;
                Description = description;
                Value = value;
            }
        }

        List<DateTimeRange> DateTimeRanges { get; set; }
        Tuple<DateTime, DateTime> GetNewDateTimeRange(DateTimeRange dateTimeRange);
    }
}