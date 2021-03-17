using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherAndPower.Contracts;
using WeatherAndPower.UI.Commands;

namespace WeatherAndPower.UI.ViewModels.AddWindow
{
    public class MainViewModel : ViewModelBase
    {
        private IAddWindowModel _model;

        public IAddWindowModel Model
        {
            get { return _model; }
            set
            {
                if (_model != value)
                {
                    _model = value;
                    Command = new RelayCommand(() => Model.RadioButtonAction());
                }
            }
        }
        private ViewModelBase _selectedViewModel = new PowerInputViewModel();
        private DataFormat _dataType = DataFormat.Power;

        public DataFormat DataType
        {
            get { return _dataType; }
            set { _dataType = value; NotifyPropertyChanged("DataType"); }
        }

        


        public ICommand UpdateViewCommand { get; set; }


        public ViewModelBase SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                _selectedViewModel = value;
                NotifyPropertyChanged(nameof(SelectedViewModel));
            }
        }

        public RelayCommand Command;

        public MainViewModel()
        {
            UpdateViewCommand = new UpdateViewCommand(this);
        }
    }
}
