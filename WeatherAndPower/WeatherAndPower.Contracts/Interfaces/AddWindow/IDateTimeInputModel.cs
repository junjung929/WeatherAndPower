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
        IPreference Preference { get; set; }
        /**
         * Main container of predefined datetime range
         */
        ObservableCollection<IDateTimeRange> DateTimeRanges { get; set; }
    }
}