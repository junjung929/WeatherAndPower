using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
    public interface IDateTimeRange
    {
        string Name { get; set; }
        string Description { get; set; }
        string Value { get; set; }
        bool IsEnabled { get; set; }
        bool IsSelected { get; set; }
    }
}
