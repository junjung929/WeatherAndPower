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
        public enum DataTypeEnum
        {
            Power = 0x01,
            Weather = 0x02
        }

        private ViewModelBase _selectedViewModel;
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
        public ICommand UpdateDateTimeCommand { get; set; }


        public ViewModelBase SelectedViewModel
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

        private bool _isStartTimePickerEnabled = true;
        private bool _isEndTimePickerEnabled = true;
        public bool IsStartTimePickerEnabled
        {
            get { return _isStartTimePickerEnabled; }
            set
            {
                _isStartTimePickerEnabled = value;
                NotifyPropertyChanged(nameof(IsStartTimePickerEnabled));
            }
        }
        public bool IsEndTimePickerEnabled
        {
            get { return _isEndTimePickerEnabled; }
            set
            {
                _isEndTimePickerEnabled = value;
                NotifyPropertyChanged(nameof(IsEndTimePickerEnabled));
            }
        }

        private DateTime _startTime = DateTime.Now;
        private DateTime _endTime = DateTime.Now;

        public DateTime StartTime
        {
            get { return _startTime; }
            set { _startTime = value; NotifyPropertyChanged("StartTime"); }
        }

        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; NotifyPropertyChanged("EndTime"); }
        }

        public DateTime DateTimeMin { get; set; } = DateTime.Today.AddYears(-2);
        public DateTime DateTimeMax { get; set; } = DateTime.Today.AddMonths(2).AddTicks(-1);

        public string PlotName { get; set; }
        public MainViewModel(PlaceholderViewModel viewModel)
        {
            UpdateViewCommand = new UpdateViewCommand(this);
            UpdateDateTimeCommand = new UpdateDateTimeCommand(this);
            AddPlotCommand = new AddPlotCommand(this, viewModel);
            UpdateViewCommand.Execute(DataType);
        }
    }
}
