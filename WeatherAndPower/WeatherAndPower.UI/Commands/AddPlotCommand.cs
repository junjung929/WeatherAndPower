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
                
                if (powerViewModel.SPowerType == null)
                {
                    System.Windows.MessageBox.Show("Please choose the category");
                    return;
                }
                if (powerViewModel.StartTime.Equals(powerViewModel.EndTime))
                {
                    System.Windows.MessageBox.Show("Please choose valid time range");
                    return;
                }
                if (powerViewModel.PlotName == null || powerViewModel.PlotName == "")
                {
                    System.Windows.MessageBox.Show("Please type the name of new plot");
                    return;
                }

                _placeholderViewModel.AddPowerDataToPlotCommand(powerViewModel.SPowerType, powerViewModel.StartTime, powerViewModel.EndTime, powerViewModel.PlotName);
                var window = (System.Windows.Window)parameter;
                window.Close();
            }
        }
    }
}
