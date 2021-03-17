using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherAndPower.UI.Views;

namespace WeatherAndPower.UI.Commands
{
    class OpenAddWindowCommand : ICommand
    {
        private PlaceholderViewModel viewModel;

        public OpenAddWindowCommand(PlaceholderViewModel viewModel)
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
            var addWindow = new AddWindow(viewModel);
            addWindow.Show();
        }
    }
}
