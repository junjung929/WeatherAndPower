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

        ObservableCollection<IDateTimeRange> DateTimeRanges { get; set; }
        
        void EnableDateTimeRange(IDateTimeRange dateTimeRange, bool isEnabled);
    }
}