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

namespace WeatherAndPower.UI.ViewModels.AddWindow
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
        private IAddWindowModel.DataTypeEnum _dataType = (IAddWindowModel.DataTypeEnum)0x02;

        public IAddWindowModel.DataTypeEnum DataType
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

        public ObservableCollection<IAddWindowModel.DataTypeEnum> DataTypes { get; }
            = new ObservableCollection<IAddWindowModel.DataTypeEnum>(
                Enum.GetValues(typeof(IAddWindowModel.DataTypeEnum))
                .Cast<IAddWindowModel.DataTypeEnum>());

        public string PlotName { get; set; }


        public RelayCommand UpdateViewCommand => new RelayCommand(() => UpdateSelectedViewModel(DataType));
        public RelayCommand AddGraphCommand => new RelayCommand(() => { 
            if(DataType == IAddWindowModel.DataTypeEnum.Power)
            {
            }
            else
            {

            }
        });


        public AddWindowViewModel(IAddWindowModel model)
        {
            _model = model;
            UpdateSelectedViewModel(DataType);
        }
    }
}
