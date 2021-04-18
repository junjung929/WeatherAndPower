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
        /**
         * Main container for PowerType
         */
        ObservableCollection<PowerType> PowerTypes { get; }
        List<Interval> Intervals { get; }

        /**
         * Return available power services corresponding to selected power source
         */
        List<PowerType.ServiceEnum> GetUpdatedPowerServices(PowerType.SourceEnum powerSource);
        /**
         * Return available power parameters corresponding to selected power source and service
         */
        List<PowerType.ParameterEnum> GetUpdatedPowerParameters(PowerType.SourceEnum powerSource, PowerType.ServiceEnum powerService);
        /**
         * Return the corresponding powerType to selected power source, service and parameter
         */
        PowerType GetUpdatedPowerType(PowerType.SourceEnum powerSource, PowerType.ServiceEnum powerService, PowerType.ParameterEnum powerParameter);
        /**
         * Check if the PowerType is real time data
         */
        bool CheckIsRealTimeParameter(PowerType.ParameterEnum powerParameter);
        /**
         * Get the intervals that have bigger value than minInterval
         */
        List<Interval> GetUpdatedIntervals(int minInterval);
        /**
         * Create and return new DateTimeInputModel
         */
        IDateTimeInputModel CreateDateTimeInputModel();

    }
}
