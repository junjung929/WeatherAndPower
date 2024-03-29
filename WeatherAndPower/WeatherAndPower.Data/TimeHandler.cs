﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeatherAndPower.Data
{
    public static class TimeHandler
    {
        /*
         * APIs handle time in UTC
         */
        public static DateTime ConvertToLocalTime(DateTime time)
        {
            return TimeZoneInfo.ConvertTimeToUtc(time);
        }

        /*
         * Checks the number of datapoints based on provided start and end times is larger that BIGDATA
         * Shows a warning and asks if user wants to proceed anyway. Based on user response 
         * execution either continues or stops. 
         */
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

        /*
         * If the starttime of forecast is too far in the future and is not handled by the api,
         * shows a warning, otherwise continues execution. 
         */
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
        /*
         * 
         * Splits a large FMI request into smaller parts 
         * and returns week-long chunks of time that api can handle. 
         */
        public static List<Tuple<DateTime, DateTime>> SplitFMIRequest(DateTime start, DateTime end)
        {

            // All my homies hate tuples

            TimeSpan max_span = new TimeSpan(7, 0, 0, 0);
            TimeSpan query_span = end - start;
            DateTime temp;
            List<Tuple<DateTime, DateTime>> split_times = new List<Tuple<DateTime, DateTime>>();
            if (query_span > max_span)
            {
                while (start < end)
                {
                    temp = start;
                    TimeSpan new_span = end - start;
                    if (new_span < max_span)
                    {
                        AddTimePair(ref split_times, start, end);
                        return split_times;
                    }
                    else
                    {
                        start += max_span;
                        AddTimePair(ref split_times, temp, start);
                    }
                }
                return split_times;
            }
            // If the span is OK to begin with
            AddTimePair(ref split_times, start, end);
            return split_times;

        }
        /*
         * Helper func for SplitFMI
         */
        private static void AddTimePair(ref List<Tuple<DateTime, DateTime>> split_times, DateTime start, DateTime end)
        {
            Tuple<DateTime, DateTime> timepair = new Tuple<DateTime, DateTime>(start, end);
            split_times.Add(timepair);
        }

        /*
         * Checks if the selected time interval is valid. Shows a warning or continues
         */
        public static bool IsTimeValid(DateTime startTime, DateTime endTime)
        {
            if (startTime.Equals(endTime)
                    || DateTime.Compare(startTime, endTime) > 0)
            {
                throw new Exception("Please choose valid time range");
            }
            return true;
        }
    }
}

