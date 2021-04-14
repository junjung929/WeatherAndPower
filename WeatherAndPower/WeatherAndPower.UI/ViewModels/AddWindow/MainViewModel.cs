using System;
using System.Collections.Generic;
using System.Globalization;
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
        public enum DataTypeEnum
        {
            Power = 0x01,
            Weather = 0x02
        }

        private InputViewModelBase _selectedViewModel;
        private DataTypeEnum _dataType = (DataTypeEnum)0x01;

        public DataTypeEnum DataType
        {
            get { return _dataType; }
            set
            {
                _dataType = value; NotifyPropertyChanged("DataType");
                UpdateViewCommand.Execute(DataType);
            }
        }


        public ICommand UpdateViewCommand { get; set; }
        public ICommand AddPlotCommand { get; set; }


        public InputViewModelBase SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                _selectedViewModel = value;
                NotifyPropertyChanged(nameof(SelectedViewModel));
            }
        }

        public Array DataTypes
        {
            get { return Enum.GetValues(typeof(DataTypeEnum)); }
        }

        public string PlotName { get; set; }
        public MainViewModel(PlaceholderViewModel viewModel)
        {
            UpdateViewCommand = new UpdateViewCommand(this);
            AddPlotCommand = new AddPlotCommand(this, viewModel);
            UpdateViewCommand.Execute(DataType);
        }
    }
}
