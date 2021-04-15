using System;

namespace WeatherAndPower.Contracts
{
    public interface IAddWindowModel
    {
        enum DataTypeEnum
        {
            Power = 0x01,
            Weather = 0x02
        }


        void AddPowerGraphAction(
            PowerType powerType,
            DateTime startTime,
            DateTime endTime,
            string graphName);

        void AddWeatherGraph(
            string cityName,
            string parameters,
            DateTime startTime,
            DateTime endTime,
            string graphName,
            WeatherType.ParameterEnum parameterType);

        IPowerInputModel CreateNewPowerInputModel();
        IWeatherInputModel CreateNewWeatherInputModel();
    }
}
