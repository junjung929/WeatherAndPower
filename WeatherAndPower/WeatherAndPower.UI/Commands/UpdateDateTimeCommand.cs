using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherAndPower.UI;

namespace WeatherAndPower.UI.Commands
{
    public class UpdateDateTimeCommand : ICommand
    {
        private DateTimeViewModel viewModel;

        public UpdateDateTimeCommand(DateTimeViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            string dateTimeRange = (string)parameter;
            DateTime now = DateTime.Now;
            DateTime today = DateTime.Now.Date;
            DateTime startTime = now;
            DateTime endTime = now;
            if (dateTimeRange == "pyear")
            {
                startTime = today.AddDays(-365);
                endTime = today.AddTicks(-1);
            }
            else if (dateTimeRange == "pmonth")
            {
                startTime = today.AddDays(-30);
                endTime = today.AddTicks(-1);
            }
            else if (dateTimeRange == "pweek")
            {
                startTime = today.AddDays(-7);
                endTime = today.AddTicks(-1);
            }
            else if (dateTimeRange == "p24h")
            {
                startTime = today.AddHours(now.Hour - 24);
                endTime = today.AddHours(now.Hour).AddTicks(-1);
            }
            else if (dateTimeRange == "n24h")
            {
                startTime = today.AddHours(now.Hour + 1);
                endTime = startTime.AddHours(24).AddTicks(-1);
            }
            else if (dateTimeRange == "n36h")
            {
                startTime = today.AddHours(now.Hour + 1);
                endTime = startTime.AddHours(36).AddTicks(-1);
            }
            else if (dateTimeRange == "n7d")
            {
                startTime = today.AddDays(1);
                endTime = startTime.AddDays(7).AddTicks(-1);
            }
            else if (dateTimeRange == "n30d")
            {
                startTime = today.AddDays(1);
                endTime = startTime.AddDays(30).AddTicks(-1);
            }
            else if (dateTimeRange == "lyear")
            {
                startTime = new DateTime(today.Year - 1, 1, 1);
                endTime = startTime.AddYears(1).AddTicks(-1);
            }
            else if (dateTimeRange == "lmonth")
            {
                DateTime lastMonth = today.AddMonths(-1);
                startTime = new DateTime(lastMonth.Year, lastMonth.Month, 1);
                endTime = startTime.AddMonths(1).AddTicks(-1);
            }
            else if (dateTimeRange == "yesterday")
            {
                startTime = today.AddDays(-1);
                endTime = startTime.AddDays(1).AddTicks(-1);
            }
            else if (dateTimeRange == "today")
            {
                startTime = today;
                endTime = startTime.AddDays(1).AddTicks(-1);
            }
            else if (dateTimeRange == "tomorrow")
            {
                startTime = today.AddDays(1);
                endTime = startTime.AddDays(1).AddTicks(-1);
            }
            else if (dateTimeRange == "tmonth")
            {
                startTime = new DateTime(today.Year, today.Month, 1);
                endTime = startTime.AddMonths(1).AddTicks(-1);
            }
            else if (dateTimeRange == "tyear")
            {
                startTime = new DateTime(today.Year, 1, 1);
                endTime = startTime.AddYears(1).AddTicks(-1);
            }

            if (viewModel.IsStartTimePickerEnabled)
            {
                var isValid = viewModel.CheckDateTimeValid(startTime);
                if (isValid < 0)
                {
                    startTime = viewModel.DateTimeMin;
                }
                else if (isValid > 0)
                {
                    startTime = viewModel.DateTimeMax;
                }
                viewModel.StartTime = startTime;
            }
            if (viewModel.IsEndTimePickerEnabled)
            {
                var isValid = viewModel.CheckDateTimeValid(endTime);
                if (isValid < 0)
                {
                    endTime = viewModel.DateTimeMin;
                }
                else if (isValid > 0)
                {
                    endTime = viewModel.DateTimeMax;
                }
                viewModel.EndTime = endTime;
            }
        }
    }
}
