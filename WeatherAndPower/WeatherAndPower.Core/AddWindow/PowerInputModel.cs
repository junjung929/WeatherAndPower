using System;
using System.Collections.Generic;
using System.Linq;
using WeatherAndPower.Contracts;

namespace WeatherAndPower.Core
{
    public class PowerInputModel : AbstractModel, IPowerInputModel
    {
        public List<PowerType> PowerTypes { get; } = new List<PowerType>(PowerType.GetAll<PowerType>());

        public List<Interval> Intervals { get; } = new List<Interval>()
        {
            new Interval(3),
            new Interval(30),
            new Interval(60),
            new Interval(360),
            new Interval(720),
            new Interval(1440),
            new Interval(1440*7),
            new Interval(1440*30)
        };

        public bool CheckIsRealTimeParameter(PowerType.ParameterEnum powerParameter)
        {
            if (powerParameter == PowerType.ParameterEnum.RealTime) return true;
            return false;
        }

        public IDateTimeInputModel CreateDateTimeInputModel()
        {
            return new DateTimeInputModel();
        }

        public List<Interval> GetUpdatedIntervals(int minInterval)
        {
            var intervals = new List<Interval>();
            Intervals.ForEach(interval =>
            {
                if (interval.Value < minInterval)
                {
                    intervals.Add(new Interval(interval.Value) { IsEnabled = false });
                }
                else
                {
                    intervals.Add(new Interval(interval.Value));
                }
            });
            return intervals;
        }

        public List<PowerType.ParameterEnum> GetUpdatedPowerParameters(PowerType.SourceEnum powerSource, PowerType.ServiceEnum powerService)
        {
            var selectableTypes = PowerTypes.ToList().FindAll(powerType => {
                return powerType.Source == powerSource && powerType.Service == powerService;
            }
            );
            var selectableParameters = selectableTypes.Select(selectableType => selectableType.ParameterType).Distinct();
            return new List<PowerType.ParameterEnum>(selectableParameters);
        }

        public List<PowerType.ServiceEnum> GetUpdatedPowerServices(PowerType.SourceEnum powerSource)
        {
            var selectableTypes = PowerTypes.ToList().FindAll(powerType => powerType.Source == powerSource);
            var selectableServices = selectableTypes.Select(selectableType => selectableType.Service).Distinct();
            return new List<PowerType.ServiceEnum>(selectableServices);
        }

        public PowerType GetUpdatedPowerType(PowerType.SourceEnum powerSource, PowerType.ServiceEnum powerService, PowerType.ParameterEnum powerParameter)
        {
            return PowerTypes.ToList().Find(powerType =>
                powerType.Source == powerSource
                && powerType.Service == powerService
                && powerType.ParameterType == powerParameter);
        }
    }
}
