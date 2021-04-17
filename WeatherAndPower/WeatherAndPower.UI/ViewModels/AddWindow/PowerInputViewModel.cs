using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WeatherAndPower.Contracts;

namespace WeatherAndPower.UI
{
    public class PowerInputViewModel : InputViewModelBase
    {
        private IPowerInputModel _Model;

        public IPowerInputModel Model
        {
            get { return _Model; }
            private set
            {
                if (_Model != value)
                {
                    _Model = value;
                }
            }
        }

        private bool _IsRealTime { get; set; } = false;
        public bool IsRealTime
        {
            get { return _IsRealTime; }
            set
            {
                if (_IsRealTime != value)
                {
                    _IsRealTime = value;
                    NotifyPropertyChanged("IsRealTime");
                }
            }
        }

        public ObservableCollection<PowerType.SourceEnum> PowerSources { get; set; } 
            = new ObservableCollection<PowerType.SourceEnum>(Enum.GetValues(typeof(PowerType.SourceEnum)).Cast<PowerType.SourceEnum>());

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

        private ObservableCollection<PowerType.ServiceEnum> _PowerServices { get; set; } = new ObservableCollection<PowerType.ServiceEnum>();
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

        private ObservableCollection<PowerType.ParameterEnum> _PowerParameters { get; set; } = new ObservableCollection<PowerType.ParameterEnum>();
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

        public ObservableCollection<PowerType> PowerTypes { get { return Model.PowerTypes; } }
        private PowerType SelectedPowerType { get; set; }
        public PowerType SPowerType
        {
            get { return SelectedPowerType; }
            set
            {
                SelectedPowerType = value;
                NotifyPropertyChanged("SPowerType");
            }
        }

        private ObservableCollection<Interval> _Intervals { get; set; } = new ObservableCollection<Interval>();


        public ObservableCollection<Interval> Intervals
        {
            get { return _Intervals; }
            set
            {
                _Intervals = value;
                NotifyPropertyChanged("Intervals");
            }
        }

        private Interval _SelectedInterval { get; set; }
        public Interval SelectedInterval
        {
            get { return _SelectedInterval; }
            set { _SelectedInterval = value; NotifyPropertyChanged("SelectedInterval"); }
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

        int _MinInterval { get; set; } = 3;
        public void UpdateIntervals()
        {
            int minInterval = SPowerType.Interval;

            if (minInterval != _MinInterval)
            {
                _MinInterval = minInterval;
                Intervals = new ObservableCollection<Interval>(Model.GetUpdatedIntervals(minInterval));
                SelectedInterval = Intervals.ToList().Find(interval => interval.Value >= minInterval); //SelectedInterval.Value < minInterval ? Intervals.ToList().Find(interval => interval.Value >= minInterval) : SelectedInterval;
            }
        }



        public void OnUpdateSelectedPowerSource()
        {
            Console.WriteLine("Power source " + SelectedPowerSource);
            UpdatePowerServices();
        }

        public void OnUpdateSelectedPowerService()
        {
            Console.WriteLine("Power service " + SelectedPowerSource);
            UpdatePowerParameters();
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
        }

        public RelayCommand UpdatePowerSourceCommand => new RelayCommand(
                () => OnUpdateSelectedPowerSource());
        public RelayCommand UpdatePowerServiceCommand => new RelayCommand(
                () => OnUpdateSelectedPowerService());
        public RelayCommand UpdatePowerParameterCommand => new RelayCommand(
                () => OnUpdateSelectedPowerParameter());

        public PowerInputViewModel(IPowerInputModel model)
        {
            _Model = model;
            CreateDateTimeViewModel();
            SelectedPowerSource = PowerType.SourceEnum.All;
        }

    }
}
