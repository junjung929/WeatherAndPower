using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherAndPower.Contracts;
using WeatherAndPower.UI.Commands;
using static WeatherAndPower.Contracts.IAddWindowModel;

namespace WeatherAndPower.UI
{
    public class AddWindowViewModel : ViewModelBase
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

        private InputViewModelBase _selectedViewModel;
        private DataTypeEnum _dataType = (DataTypeEnum)0x02;

        public DataTypeEnum DataType
        {
            get { return _dataType; }
            set
            {
                _dataType = value; NotifyPropertyChanged("DataType");
                UpdateViewCommand.Execute(DataType);
            }
        }

        private void UpdateSelectedViewModel(IAddWindowModel.DataTypeEnum dataType)
        {
            if (dataType == IAddWindowModel.DataTypeEnum.Power)
            {
                var powerModel = Model.CreateNewPowerInputModel();
                SelectedViewModel = new PowerInputViewModel(powerModel);
            }
            else
            {
                var weatherModel = Model.CreateNewWeatherInputModel();
                SelectedViewModel = new WeatherInputViewModel(weatherModel);
            }
        }

        public InputViewModelBase SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                _selectedViewModel = value;
                NotifyPropertyChanged(nameof(SelectedViewModel));
            }
        }

        public ObservableCollection<DataTypeEnum> DataTypes { get; }
           = new ObservableCollection<DataTypeEnum>(
               Enum.GetValues(typeof(DataTypeEnum))
               .Cast<DataTypeEnum>());

        public RelayCommand UpdateViewCommand => new RelayCommand(() => UpdateSelectedViewModel(DataType));
        public RelayCommand AddGraphCommand => new RelayCommand(() => {
            if (DataType == IAddWindowModel.DataTypeEnum.Power)
            {
            }
            else
            {

            }
        });

        public string PlotName { get; set; }
        public AddWindowViewModel(IAddWindowModel model)
        {
            _model = model;
            UpdateSelectedViewModel(DataType);
        }
    }
}
