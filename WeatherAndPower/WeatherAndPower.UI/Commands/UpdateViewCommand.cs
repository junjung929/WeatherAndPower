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
    public class UpdateViewCommand : ICommand
    {
        private MainViewModel _viewModel { get; set; }

        public UpdateViewCommand(MainViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter.ToString() == MainViewModel.DataTypeEnum.Power.ToString())
            {
                _viewModel.SelectedViewModel = new PowerInputViewModel(_viewModel);
            }
            else
            {
                _viewModel.SelectedViewModel = new WeatherInputViewModel(_viewModel);
            }
            _viewModel.SelectedViewModel.DateTimeViewModel = new DateTimeViewModel(_viewModel.SelectedViewModel);
            _viewModel.SelectedViewModel.UpdateDateTimeMinMax();
        }
    }
}
