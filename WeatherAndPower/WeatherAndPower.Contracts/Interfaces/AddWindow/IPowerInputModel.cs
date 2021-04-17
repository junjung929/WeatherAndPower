using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
    public interface IPowerInputModel
    {
        IPowerPreference Preference { get; set; }
        /**
         * Main container for PowerType
         */
        ObservableCollection<PowerType> PowerTypes { get; }
        ObservableCollection<PowerType.SourceEnum> PowerSources{ get;  }
        ObservableCollection<PowerType.ServiceEnum> PowerServices { get;  }
        ObservableCollection<PowerType.ParameterEnum> PowerParameters{ get;  }

        List<Interval> Intervals { get; }


        /**
         * Updatee available power sources
         */
        void UpdatePowerSources();
        /**
         * Updatee available power services corresponding to selected power source
         */
        void UpdatePowerServices();
        /**
         * Updatee available power parameters corresponding to selected power source and service
         */
        void UpdatePowerParameters();
        /**
         * Updatee the corresponding powerType to selected power source, service and parameter
         */
        void UpdatePowerType();
        /**
         * Disable intervals that have smaller value than minInterval
         */
        List<Interval> GetUpdatedIntervals(int minInterval);

        /**
         * Check if the PowerType is real time data
         */
        bool CheckIsRealTimeParameter(PowerType.ParameterEnum powerParameter);
        
        /**
         * Create and return new DateTimeInputModel
         */
        IDateTimeInputModel CreateDateTimeInputModel(IPreference preference);

    }
}
