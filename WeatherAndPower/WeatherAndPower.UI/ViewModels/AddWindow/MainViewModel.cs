using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherAndPower.UI.Commands;

namespace WeatherAndPower.UI.ViewModels.AddWindow
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel _selectedViewModel = new PowerInputViewModel();

        public MainViewModel()
        {
            UpdateViewCommand = new UpdateViewCommand(this);
        }


        public ICommand UpdateViewCommand { get; set; }


        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set 
            { 
                _selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }
    }
}
