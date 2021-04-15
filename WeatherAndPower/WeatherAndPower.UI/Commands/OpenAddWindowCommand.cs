using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherAndPower.UI;

namespace WeatherAndPower.UI.Commands
{
    class OpenAddWindowCommand : ICommand
    {
        private SidebarViewModel viewModel;

        public OpenAddWindowCommand(SidebarViewModel viewModel)
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
            //var addWindow = new AddWindow(viewModel);
            //addWindow.Show();
        }
    }
}
