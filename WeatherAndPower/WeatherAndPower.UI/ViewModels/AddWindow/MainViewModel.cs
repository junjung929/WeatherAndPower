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
        private ViewModelBase _selectedViewModel;
        private DataFormat _dataType = (DataFormat)0x01;

        public DataFormat DataType
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
            get { return Enum.GetValues(typeof(DataFormat)); }
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
        public string PlotName { get; set; }
        public MainViewModel(PlaceholderViewModel viewModel)
        {
            UpdateViewCommand = new UpdateViewCommand(this);
            AddPlotCommand = new AddPlotCommand(this, viewModel);
            UpdateViewCommand.Execute(DataType);
        }
    }
}
