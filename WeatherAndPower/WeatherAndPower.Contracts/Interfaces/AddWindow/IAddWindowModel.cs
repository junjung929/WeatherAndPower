using System;

namespace WeatherAndPower.Contracts
{
    public interface IAddWindowModel
    {
        IPreference Preference { get; set; }
        enum DataTypeEnum
        {
            Power = 0x01,
            Weather = 0x02
        }

        /**
         * Add power data ot the graph
         */
        void AddPowerGraphAction();

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


        IPowerPreference CreateNewPowerPreference();

        /**
         * Create and return new PowerInputModel
         */
        IPowerInputModel CreateNewPowerInputModel(IPowerPreference preference);
        /**
        * Create and return new WeatherInputModel
        */
        IWeatherInputModel CreateNewWeatherInputModel(IWeatherPreference preference);
    }
}
