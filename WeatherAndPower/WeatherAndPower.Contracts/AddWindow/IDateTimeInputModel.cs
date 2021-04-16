using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
    public interface IDateTimeInputModel
    {
        enum EDateTimeFormat
        {
            StartTime = 0x01,
            EndTime = 0x02
        }
        DateTime DefaultDateTimeMin { get; }
        DateTime DefaultDateTimeMax { get; }
        DateTime StartTime { get; set; }
        DateTime EndTime { get; set; }
        DateTime DateTimeMin { get; set; }
        DateTime DateTimeMax { get; set; }

        ObservableCollection<IDateTimeRange> DateTimeRanges { get; set; }


        void UpdateDateTimeMinMaxToDefault();

        /// <summary>
        /// Updates minimum datetime and maximum datetime values
        /// </summary>
        /// <param name="min">New minimum datetime value to set</param>
        /// <param name="max">New maximum datetime value to set</param>
        void UpdateDateTimeMinMax(DateTime min, DateTime max);

        void UpdateDateTimes(IDateTimeRange dateTimeRange);
        Tuple<DateTime, DateTime> GetNewDateTimeRange(IDateTimeRange dateTimeRange);

        void EnableDateTimeRange(IDateTimeRange dateTimeRange, bool isEnabled);
    }
}