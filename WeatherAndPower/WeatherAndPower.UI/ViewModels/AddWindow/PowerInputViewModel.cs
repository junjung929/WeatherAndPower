using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WeatherAndPower.Contracts;

namespace WeatherAndPower.UI
{
    public class PowerInputViewModel : InputViewModelBase
    {
        #region Properties
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

        public ObservableCollection<PowerType.SourceEnum> PowerSources
        {
            get { return Model.PowerSources; }
        }

        public PowerType.SourceEnum SelectedPowerSource
        {
            get { return Model.Preference.PowerSource; }
            set
            {
                if (Model.Preference.PowerSource != value)
                {
                    Model.Preference.PowerSource = value;
                    NotifyPropertyChanged("SelectedPowerSource");
                    OnUpdateSelectedPowerSource();
                }
            }
        }

        public ObservableCollection<PowerType.ServiceEnum> PowerServices
        {
            get { return Model.PowerServices; }
        }

        public PowerType.ServiceEnum SelectedPowerService
        {
            get { return Model.Preference.PowerService; }
            set
            {
                Model.Preference.PowerService = value;
                NotifyPropertyChanged("SelectedPowerService");
                OnUpdateSelectedPowerService();
            }
        }

        public ObservableCollection<PowerType.ParameterEnum> PowerParameters
        {
            get { return Model.PowerParameters; }
        }

        private PowerType.ParameterEnum _SelectedPowerParameter { get; set; }
        public PowerType.ParameterEnum SelectedPowerParameter
        {
            get { return Model.Preference.PowerParameter; }
            set
            {
                Model.Preference.PowerParameter = value;
                NotifyPropertyChanged("SelectedPowerParameter");
                OnUpdateSelectedPowerParameter();
            }
        }

        public ObservableCollection<PowerType> PowerTypes
        {
            get { return Model.PowerTypes; }
        }

        public PowerType SPowerType
        {
            get { return Model.Preference.PowerType; }
            set
            {
                Model.Preference.PowerType = value;
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
            set
            {
                _SelectedInterval = value;
                Model.Preference.Interval = value;
                NotifyPropertyChanged("SelectedInterval");
            }
        }
        #endregion

        public void UpdateRealTime()
        {
            IsRealTime = Model.CheckIsRealTimeParameter(SelectedPowerParameter);
        }

        public override void UpdateDateTimeMinMax()
        {
            if (SelectedPowerParameter == PowerType.ParameterEnum.RealTime ||
                SelectedPowerParameter == PowerType.ParameterEnum.Observation)
            {
                DateTimeViewModel.UpdateDateTimeMinMax(DateTimeViewModel.DefaultDateTimeMin, 
                    DateTime.Today.AddHours(DateTime.Now.Hour + 1).AddTicks(-1));

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
                // only grab one available option
                Intervals = new ObservableCollection<Interval>() { Model.GetUpdatedIntervals(minInterval).Where(e => e.IsEnabled).First() };
                SelectedInterval = Intervals.First(); //SelectedInterval.Value < minInterval ? Intervals.ToList().Find(interval => interval.Value >= minInterval) : SelectedInterval;
            }
        }



        public void OnUpdateSelectedPowerSource()
        {
            Console.WriteLine("Power source " + SelectedPowerSource);
            Model.UpdatePowerServices();
            NotifyPropertyChanged("SelectedPowerService");
            OnUpdateSelectedPowerService();
        }

        public void OnUpdateSelectedPowerService()
        {
            Console.WriteLine("Power service " + SelectedPowerSource);
            Model.UpdatePowerParameters();
            NotifyPropertyChanged("SelectedPowerParameter");
            OnUpdateSelectedPowerParameter();
        }

        public void OnUpdateSelectedPowerParameter()
        {
            Console.WriteLine("Power parameter " + SelectedPowerParameter);
            UpdateRealTime();
            Model.UpdatePowerType();
            NotifyPropertyChanged("SPowerType");

            UpdateDateTimeMinMax();
            UpdateIntervals();
        }

        public override void CreateDateTimeViewModel()
        {
            var dateTimeInputModel = Model.CreateDateTimeInputModel(Model.Preference);
            DateTimeViewModel = new DateTimeViewModel(dateTimeInputModel);
        }

        private void InitializeComponent()
        {
            CreateDateTimeViewModel();
            Model.UpdatePowerServices();
            Model.UpdatePowerParameters();
            Model.UpdatePowerType();
            UpdateDateTimeMinMax();
            UpdateIntervals();

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
            InitializeComponent();
        }

    }
}
