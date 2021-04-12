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
            set
            {
                if (DateTime.Compare(value, DateTimeMin) < 0)
                {
                    System.Windows.MessageBox.Show("The minimum range of start date time is "
                        + DateTimeMin.ToString("ddd dd'/'MM'/'yyyy HH:mm:ss", new CultureInfo("en-US")));
                    _startTime = DateTimeMin;
                }
                else if (DateTime.Compare(value, DateTimeMax) > 0)
                {
                    System.Windows.MessageBox.Show("The maximum range of start date time is "
                       + DateTimeMax.ToString("ddd dd'/'MM'/'yyyy HH:mm:ss", new CultureInfo("en-US")));
                    _startTime = DateTimeMax;
                }
                else _startTime = value;
                NotifyPropertyChanged("StartTime");
            }
        }

        public DateTime EndTime
        {
            get { return _endTime; }
            set
            {
                if (DateTime.Compare(value, DateTimeMin) < 0)
                {
                    System.Windows.MessageBox.Show("The minimum range of end date time is "
                        + DateTimeMin.ToString("ddd dd'/'MM'/'yyyy HH:mm:ss", new CultureInfo("en-US")));
                    _endTime = DateTimeMin;
                }
                else if (DateTime.Compare(value, DateTimeMax) > 0)
                {
                    System.Windows.MessageBox.Show("The maximum range of end date time is "
                        + DateTimeMax.ToString("ddd dd'/'MM'/'yyyy HH:mm:ss", new CultureInfo("en-US")));
                    _endTime = DateTimeMax;
                }
                else _endTime = value;
                NotifyPropertyChanged("EndTime");
            }
        }

        public DateTime DefaultDateTimeMin { get; private set; } = DateTime.Today.AddYears(-2);
        public DateTime DefaultDateTimeMax { get; private set; } = DateTime.Today.AddMonths(2).AddTicks(-1);

        private DateTime _dateTimeMin;
        private DateTime _dateTimeMax;
        public DateTime DateTimeMin
        {
            get { return _dateTimeMin; }
            set { _dateTimeMin = value; NotifyPropertyChanged("DateTimeMin"); }
        }
        public DateTime DateTimeMax
        {
            get { return _dateTimeMax; }
            set { _dateTimeMax = value; NotifyPropertyChanged("DateTimeMax"); }
        }


        public void SetDateTimeMinMaxToDefault()
        {
            DateTimeMin = DefaultDateTimeMin;
            DateTimeMax = DefaultDateTimeMax;
        }
        public string PlotName { get; set; }
        public MainViewModel(PlaceholderViewModel viewModel)
        {
            SetDateTimeMinMaxToDefault();
            UpdateViewCommand = new UpdateViewCommand(this);
            UpdateDateTimeCommand = new UpdateDateTimeCommand(this);
            AddPlotCommand = new AddPlotCommand(this, viewModel);
            UpdateViewCommand.Execute(DataType);
        }
    }
}
