using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
    public interface IPowerPreference : IPreference
    {
        PowerType.SourceEnum PowerSource { get; set; }
        PowerType.ServiceEnum PowerService { get; set; }
        PowerType.ParameterEnum PowerParameter { get; set; }

        PowerType PowerType { get; set; }
    }
}
