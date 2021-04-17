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

        /**
         * Add power data ot the graph
         */
        void AddPowerGraphAction(
            PowerType powerType,
            DateTime startTime,
            DateTime endTime,
            string graphName);

        /**
         * Add weather data ot the graph
         */
        void AddWeatherGraph(
            string cityName,
            string parameters,
            DateTime startTime,
            DateTime endTime,
            string graphName,
            WeatherType.ParameterEnum parameterType,
            int interval);


        /**
         * Create and return new PowerInputModel
         */
        IPowerInputModel CreateNewPowerInputModel();
        /**
        * Create and return new WeatherInputModel
        */
        IWeatherInputModel CreateNewWeatherInputModel();
    }
}
