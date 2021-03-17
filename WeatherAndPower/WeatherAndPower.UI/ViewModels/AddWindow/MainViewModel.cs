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
            private set
            {
                if (_model != value)
                {
                    _model = value;
                }
            }
        }
        private BaseViewModel _selectedViewModel = new PowerInputViewModel();
        private DataFormat _dataType = DataFormat.Power;

        public DataFormat DataType
        {
            get { return _dataType; }
            set { _dataType = value; NotifyPropertyChanged("DataType"); }
        }

        


        public ICommand UpdateViewCommand { get; set; }


        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                _selectedViewModel = value;
                NotifyPropertyChanged(nameof(SelectedViewModel));
            }
        }

        //public RelayCommand Command = new RelayCommand(() => Model.RadioButtonAction());

        public MainViewModel(/*IAddWindowModel model*/)
        {
            //_model = model;
            UpdateViewCommand = new UpdateViewCommand(this);
        }
    }
}
