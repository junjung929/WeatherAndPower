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
        private PlaceholderViewModel _placeholderViewModel;

        public AddPlotCommand(MainViewModel viewModel, PlaceholderViewModel placeholderViewModel)
        {
            _viewModel = viewModel;
            _placeholderViewModel = placeholderViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (_viewModel.DataType.Equals(DataFormat.Power))
            {
                var powerViewModel = (PowerInputViewModel)_viewModel.SelectedViewModel;
                //powerViewModel.StartTime;
                System.Windows.MessageBox.Show(powerViewModel.SPowerType + powerViewModel.StartTime.ToString() + powerViewModel.StartTime.ToString() + powerViewModel.PlotName);

                _placeholderViewModel.AddPowerDataToPlotCommand(powerViewModel.SPowerType, powerViewModel.StartTime, powerViewModel.StartTime, powerViewModel.PlotName);
                var window = (System.Windows.Window)parameter;
                window.Close();
            }
        }
    }
}
