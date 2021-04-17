using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
    public interface IPowerInputModel
    {
        List<PowerType> PowerTypes { get; }
        List<Interval> Intervals { get; }

        List<PowerType.ServiceEnum> GetUpdatedPowerServices(PowerType.SourceEnum powerSource);
        List<PowerType.ParameterEnum> GetUpdatedPowerParameters(PowerType.SourceEnum powerSource, PowerType.ServiceEnum powerService);
        PowerType GetUpdatedPowerType(PowerType.SourceEnum powerSource, PowerType.ServiceEnum powerService, PowerType.ParameterEnum powerParameter);

        bool CheckIsRealTimeParameter(PowerType.ParameterEnum powerParameter);

        List<Interval> GetUpdatedIntervals(int minInterval);

        IDateTimeInputModel CreateDateTimeInputModel();

    }
}
