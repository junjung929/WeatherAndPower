using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherAndPower.Contracts;
using WeatherAndPower.UI.ViewModels.AddWindow;

namespace WeatherAndPower.UI.Commands
{
    public class AddPlotCommand : ICommand
    {
        private MainViewModel _viewModel;
        private SidebarViewModel _sidebarViewModel;

        public AddPlotCommand(MainViewModel viewModel, SidebarViewModel sidebarViewModel)
        {
            _viewModel = viewModel;
            _sidebarViewModel = sidebarViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (_viewModel.DataType.Equals(MainViewModel.DataTypeEnum.Power))
            {
                AddPowerGraph((System.Windows.Window)parameter);
            }
            else
            {
                AddWeatherGraph((System.Windows.Window)parameter);
            }
        }

        private void AddPowerGraph(System.Windows.Window window)
        {
            var powerViewModel = (PowerInputViewModel)_viewModel.SelectedViewModel;
            Console.WriteLine(powerViewModel.SPowerType);
            if (powerViewModel.SPowerType == null)
            {
                System.Windows.MessageBox.Show("Please choose the category");
                return;
            }

            // Checks date times and plot name are valid
            if (!CommonInputValidator()) return;

            try
            {
                var startTime = _viewModel.SelectedViewModel.DateTimeViewModel.StartTime;
                var endTime = _viewModel.SelectedViewModel.DateTimeViewModel.EndTime;
                _sidebarViewModel.AddPowerGraphCommand(powerViewModel.SPowerType, startTime, endTime, _viewModel.PlotName);
                window.Close();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Failed to add the data into graph. Please try again.");
            }
        }

        private void AddWeatherGraph(System.Windows.Window window)
        {
            //var weatherViewModel = (WeatherInputViewModel)_viewModel.SelectedViewModel;
            //string parameters = String.Join(",", weatherViewModel.SelectedParameters);

            //var parameterTypes = weatherViewModel.SelectedParameters.ToList().Select(x => x.ParameterType).Distinct();

            //if (!parameterTypes.Contains(WeatherType.ParameterEnum.Forecast)
            //    && !parameterTypes.Contains(WeatherType.ParameterEnum.Observation))
            //{
            //    System.Windows.MessageBox.Show("Please choose at least one parameter from either Observation or Forecast");
            //    return;
            //}

            //if (weatherViewModel.CityName == null || weatherViewModel.CityName == "")
            //{
            //    System.Windows.MessageBox.Show("Please give a name of cities in Finland");
            //    return;
            //}

            //// Checks date times and plot name are valid
            //if (!CommonInputValidator()) return;
           
            //try
            //{
            //    var startTime = _viewModel.SelectedViewModel.DateTimeViewModel.StartTime;
            //    var endTime = _viewModel.SelectedViewModel.DateTimeViewModel.EndTime;
            //    _placeholderViewModel.AddWeatherGraphCommand(weatherViewModel.CityName, parameters, startTime, endTime, _viewModel.PlotName, weatherViewModel.SelectedParameterType);
            //    window.Close();
            //}
            //catch (Exception e)
            //{
            //    System.Windows.MessageBox.Show("Failed to add the data into graph. Please try again: " + e.Message);
            //}
        }
        private Boolean CommonInputValidator()
        {
            if (!IsTimeValid()) return false;
            if (!IsPlotNameValid()) return false;
            return true;
        }

        private Boolean IsTimeValid()
        {
            var startTime = _viewModel.SelectedViewModel.DateTimeViewModel.StartTime;
            var endTime = _viewModel.SelectedViewModel.DateTimeViewModel.EndTime;
            if (startTime.Equals(endTime)
                    || DateTime.Compare(startTime, endTime) > 0)
            {
                System.Windows.MessageBox.Show("Please choose valid time range");
                return false;
            }
            return true;
        }
        private Boolean IsPlotNameValid()
        {
            if (_viewModel.PlotName == null || _viewModel.PlotName == "")
            {
                System.Windows.MessageBox.Show("Please type the name of new plot");
                return false;
            }
            return true;
        }
    }
}
