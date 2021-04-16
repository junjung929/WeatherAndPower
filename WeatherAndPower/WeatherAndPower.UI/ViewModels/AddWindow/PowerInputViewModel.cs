using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WeatherAndPower.Contracts;

namespace WeatherAndPower.UI
{
    public class PowerInputViewModel : InputViewModelBase
    {
        private IPowerInputModel _model;

        public IPowerInputModel Model
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

        private bool _isRealTime { get; set; } = false;
        public bool IsRealTime
        {
            get { return _isRealTime; }
            set
            {
                if (_isRealTime != value)
                {
                    _isRealTime = value;
                    NotifyPropertyChanged("IsRealTime");
                }
            }
        }

        public ObservableCollection<PowerType.SourceEnum> PowerSources { get; set; }

        private PowerType.SourceEnum _SelectedPowerSource { get; set; }

        public PowerType.SourceEnum SelectedPowerSource
        {
            get { return _SelectedPowerSource; }
            set
            {
                if (_SelectedPowerSource != value)
                {
                    _SelectedPowerSource = value;
                    NotifyPropertyChanged("SelectedPowerSource");
                    OnUpdateSelectedPowerSource();
                }
            }
        }

        private ObservableCollection<PowerType.ServiceEnum> _PowerServices { get; set; }
        public ObservableCollection<PowerType.ServiceEnum> PowerServices
        {
            get { return _PowerServices; }
            set
            {
                _PowerServices = value;
                NotifyPropertyChanged("PowerServices");
            }
        }

        private PowerType.ServiceEnum _SelectedPowerService { get; set; }
        public PowerType.ServiceEnum SelectedPowerService
        {
            get { return _SelectedPowerService; }
            set
            {
                _SelectedPowerService = value;
                NotifyPropertyChanged("SelectedPowerService");
                OnUpdateSelectedPowerService();
            }
        }

        private ObservableCollection<PowerType.ParameterEnum> _PowerParameters { get; set; }
        public ObservableCollection<PowerType.ParameterEnum> PowerParameters
        {
            get { return _PowerParameters; }
            set
            {
                _PowerParameters = value;
                NotifyPropertyChanged("PowerParameters");
            }
        }

        private PowerType.ParameterEnum _SelectedPowerParameter { get; set; }
        public PowerType.ParameterEnum SelectedPowerParameter
        {
            get { return _SelectedPowerParameter; }
            set
            {
                _SelectedPowerParameter = value;
                NotifyPropertyChanged("SelectedPowerParameter");
                OnUpdateSelectedPowerParameter();
            }
        }

        public ObservableCollection<PowerType> PowerTypes { get; set; }
        private PowerType selectedPowerType { get; set; }
        public PowerType SPowerType
        {
            get { return selectedPowerType; }
            set
            {
                selectedPowerType = value;
                NotifyPropertyChanged("SPowerType");
            }
        }

        private ObservableCollection<Interval> _intervals { get; set; }


        public ObservableCollection<Interval> Intervals
        {
            get { return _intervals; }
            set
            {
                _intervals = value;
                NotifyPropertyChanged("Intervals");
            }
        }

        private Interval _selectedInterval { get; set; }
        public Interval SelectedInterval
        {
            get { return _selectedInterval; }
            set { _selectedInterval = value; NotifyPropertyChanged("SelectedInterval"); }
        }

        private void UpdatePowerServices()
        {
            PowerServices = new ObservableCollection<PowerType.ServiceEnum>(Model.GetUpdatedPowerServices(SelectedPowerSource));
            SelectedPowerService = PowerServices.First();
        }

        private void UpdatePowerParameters()
        {
            PowerParameters = new ObservableCollection<PowerType.ParameterEnum>(Model.GetUpdatedPowerParameters(SelectedPowerSource, SelectedPowerService));
            SelectedPowerParameter = PowerParameters.First();
        }

        public override void OnDateTimeUpdated(string dateTime)
        {
        }

        public void UpdateSelectedPowerType()
        {

            SPowerType = Model.GetUpdatedPowerType(SelectedPowerSource, SelectedPowerService, SelectedPowerParameter);
            Console.WriteLine(SPowerType);
        }
        public void UpdateRealTime()
        {
            IsRealTime = Model.CheckIsRealTimeParameter(SelectedPowerParameter);
        }

        public override void UpdateDateTimeMinMax()
        {
            if (SelectedPowerParameter == PowerType.ParameterEnum.RealTime ||
                SelectedPowerParameter == PowerType.ParameterEnum.Observation)
            {
                DateTimeViewModel.UpdateDateTimeMinMax(DateTimeViewModel.DefaultDateTimeMin, DateTime.Now);

            }
            else
            {
                DateTimeViewModel.UpdateDateTimeMinMax(DateTimeViewModel.DefaultDateTimeMin,
                    DateTimeViewModel.DefaultDateTimeMax);
            }
        }

        int _minInterval { get; set; } = 3;
        public void UpdateIntervals()
        {
            int minInterval = SPowerType.Interval;

            if (minInterval != _minInterval)
            {
                _minInterval = minInterval;
                Intervals = new ObservableCollection<Interval>(Model.GetUpdatedIntervals(minInterval));
                SelectedInterval = SelectedInterval.Value < minInterval ? Intervals.ToList().Find(interval => interval.Value >= minInterval) : SelectedInterval;
            }
        }



        public void OnUpdateSelectedPowerSource()
        {
            Console.WriteLine("Power source " + SelectedPowerSource);
            UpdatePowerServices();
            UpdatePowerParameters();
            UpdateRealTime();
            UpdateSelectedPowerType();
            UpdateDateTimeMinMax();
            UpdateIntervals();
        }

        public void OnUpdateSelectedPowerService()
        {
            Console.WriteLine("Power service " + SelectedPowerSource);
            UpdatePowerParameters();
            UpdateRealTime();
            UpdateSelectedPowerType();
            UpdateDateTimeMinMax();
            UpdateIntervals();
        }

        public void OnUpdateSelectedPowerParameter()
        {
            Console.WriteLine("Power parameter " + SelectedPowerParameter);
            UpdateRealTime();
            UpdateSelectedPowerType();
            UpdateDateTimeMinMax();
            UpdateIntervals();
        }

        public override void CreateDateTimeViewModel()
        {
            var dateTimeInputModel = Model.CreateDateTimeInputModel();
            DateTimeViewModel = new DateTimeViewModel(dateTimeInputModel);
            UpdateDateTimeMinMax();
        }

        public ICommand UpdatePowerSourceCommand { get; set; }
        public ICommand UpdatePowerServiceCommand { get; set; }
        public ICommand UpdatePowerParameterCommand { get; set; }

        public PowerInputViewModel(IPowerInputModel model)
        {
            _model = model;
            CreateDateTimeViewModel();
            PowerTypes = new ObservableCollection<PowerType>(model.PowerTypes);
            PowerSources = new ObservableCollection<PowerType.SourceEnum>(Enum.GetValues(typeof(PowerType.SourceEnum)).Cast<PowerType.SourceEnum>());
            SelectedPowerSource = PowerType.SourceEnum.All;
            UpdatePowerServices();
            UpdatePowerParameters();
            UpdateRealTime();
            UpdateSelectedPowerType();
            UpdateDateTimeMinMax();
            UpdateIntervals();

            UpdatePowerSourceCommand = new RelayCommand(
                () => OnUpdateSelectedPowerSource());
            UpdatePowerServiceCommand = new RelayCommand(
                () => OnUpdateSelectedPowerService());
            UpdatePowerParameterCommand = new RelayCommand(
                () => OnUpdateSelectedPowerParameter());
        }

    }
}
