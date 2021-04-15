using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherAndPower.Contracts;
using WeatherAndPower.UI;

namespace WeatherAndPower.UI.Commands
{
    public class UpdateViewCommand : ICommand
    {
        private AddWindowViewModel _viewModel { get; set; }

        public UpdateViewCommand(AddWindowViewModel viewModel)
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
            //if (parameter.ToString() == AddWindowViewModel.DataTypeEnum.Power.ToString())
            //{
            //    _viewModel.SelectedViewModel = new PowerInputViewModel(null);
            //}
            //else
            //{
            //    _viewModel.SelectedViewModel = new WeatherInputViewModel(null);
            //}
        }
    }
}
