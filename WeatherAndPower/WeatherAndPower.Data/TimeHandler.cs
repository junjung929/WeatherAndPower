using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeatherAndPower.Data
{
    public static class TimeHandler
    {

        public static DateTime ConvertToLocalTime(DateTime time)
        {
            return TimeZoneInfo.ConvertTimeToUtc(time);
        }

        public static bool DataTooBig(DateTime start, DateTime end, double timestep)
        {

            if (start > end) { return false; }

            double BIGDATA = 1000;
            double datapoints = (end - start).TotalMinutes / timestep;

            if (datapoints > BIGDATA)
            {
                DialogResult result = MessageBox.Show($"Warning, the requested dataset has {Math.Truncate(datapoints)} points \n                         Proceed anyway?", "Warning", MessageBoxButtons.YesNo);
                switch (result)
                {
                    case DialogResult.Yes:
                        return false;
                    case DialogResult.No:
                        return true;
                }
            }
            return false;
        }

        public static bool ForecastTooFar(DateTime startTime)
        {
            // 2 days 1 hour and 10 minutes from CURRENT TIME there is no forecast data available
            TimeSpan span = new TimeSpan(2, 1, 10, 0);

            if (startTime - DateTime.Now > span)
            {
                MessageBox.Show($"FMI forecast only goes {span.Days} days, {span.Hours} hour and {span.Minutes} minutes forward from now! Try setting a sooner date");
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}

