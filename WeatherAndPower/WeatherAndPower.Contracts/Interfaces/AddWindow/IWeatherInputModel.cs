using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.Contracts
{
    public interface IWeatherInputModel
    {
        enum ECity
        {
            Helsinki = 0x01,
            Tampere = 0x02,
            Oulu = 0x04,
            Rovaniemi = 0x08,
            Joensuu = 0x10,
            Turku = 0x20
        }
        IWeatherPreference Preference { get; set; }

        /**
         * Main container for WeatherType
         */
        ObservableCollection<WeatherType> WeatherTypes { get; }
        List<Interval> Intervals { get; }


        void UpdateWeatherTypes();
        /**
         * Get available WeatherTypes depending on selected weather parameter 
         */
        List<WeatherType> GetUpdatedWeatherTypes(WeatherType.ParameterEnum weatherParameter);
        /*
         * Return available intervals that have bigger value than minInterval
         */
        List<Interval> GetUpdatedIntervals(int minInterval);

        /**
         * Create and return new DateTimeInputModel
         */
        IDateTimeInputModel CreateDateTimeInputModel();
    }
}
