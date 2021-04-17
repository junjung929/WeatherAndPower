using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WeatherAndPower.Contracts.IAddWindowModel;

namespace WeatherAndPower.Contracts
{
    public interface IPreference
    {
        DataTypeEnum DataType { get; set; }


        DateTime StartTime { get; set; }
        DateTime EndTime { get; set; }
        string PlotName { get; set; }
        Interval Interval { get; set; }
    }
}
