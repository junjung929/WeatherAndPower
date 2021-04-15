using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAndPower.Contracts;

namespace WeatherAndPower.Core
{
    class AddWindowModel : AbstractModel, IAddWindowModel
    {
        public void AddGraphAction(IAddWindowModel.DataTypeEnum dataType)
        {
            throw new NotImplementedException();
        }

        public void AddPowerGraphAction(PowerType powerType, DateTime startTime, DateTime endTime, string graphName)
        {
            throw new NotImplementedException();
        }

        public void AddWeatherGraph(string cityName, string parameters, DateTime startTime, DateTime endTime, string graphName, WeatherType.ParameterEnum parameterType)
        {
            throw new NotImplementedException();
        }

        public IPowerInputModel CreateNewPowerInputModel()
        {
            return new PowerInputModel();
        }

        public IWeatherInputModel CreateNewWeatherInputModel()
        {
            return new WeatherInputModel();
        }
    }
}
