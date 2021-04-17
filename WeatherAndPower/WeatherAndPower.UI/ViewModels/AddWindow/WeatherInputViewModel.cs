using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WeatherAndPower.Contracts;
using static WeatherAndPower.Contracts.IWeatherInputModel;

namespace WeatherAndPower.UI
{
    public class WeatherInputViewModel : InputViewModelBase
    {
        private IWeatherInputModel _Model;
        public IWeatherInputModel Model
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

        private ECity _SelectedCity { get; set; } = (ECity)0x01;
        public ECity SelectedCity
        {
            get { return _SelectedCity; }
            set { _SelectedCity = value; NotifyPropertyChanged("SelectedCity"); }
        }

        public ObservableCollection<WeatherType.ParameterEnum> WeatherParameters { get; set; } = new ObservableCollection<WeatherType.ParameterEnum>()
        {
            WeatherType.ParameterEnum.Observation,
            WeatherType.ParameterEnum.Forecast
        };

        private WeatherType.ParameterEnum _SelectedParameter { get; set; } = WeatherType.ParameterEnum.Observation;
        public WeatherType.ParameterEnum SelectedParameter
        {
            get { return _SelectedParameter; }
            set
            {
                _SelectedParameter = value;
                NotifyPropertyChanged("SelectedParameter");
                OnUpdateSelectedWeatherParameter();
            }
        }

        private ObservableCollection<WeatherType> _WeatherTypes { get; set; }
        public ObservableCollection<WeatherType> WeatherTypes
        {
            get { return _WeatherTypes; }
            set { _WeatherTypes = value; NotifyPropertyChanged("WeatherTypes"); }
        }

        public List<WeatherType> SelectedWeatherTypes { get; set; } = new List<WeatherType>();

        private Visibility _MedianVisibility { get; set; } = Visibility.Visible;
        public Visibility MedianVisibility
        {
            get { return _MedianVisibility; }
            set { _MedianVisibility = value; NotifyPropertyChanged("MedianVisibility"); }
        }
        public ObservableCollection<WeatherType> _Medians { get; set; } = new ObservableCollection<WeatherType>();

        public ObservableCollection<WeatherType> Medians
        {
            get { return _Medians; }
            set { _Medians = value; NotifyPropertyChanged("Medians"); }
        }

        public List<WeatherType> SelectedMedians { get; set; } = new List<WeatherType>();

        public ObservableCollection<ECity> Cities { get; set; } = new ObservableCollection<ECity>(Enum.GetValues(typeof(ECity)).Cast<ECity>());


        private ObservableCollection<Interval> _Intervals { get; set; }

        public ObservableCollection<Interval> Intervals
        {
            get { return _Intervals; }
            set { _Intervals = value; NotifyPropertyChanged("Intervals"); }
        }

        private Interval _SelectedInterval { get; set; }
        public Interval SelectedInterval
        {
            get { return _SelectedInterval; }
            set { _SelectedInterval = value; NotifyPropertyChanged("SelectedInterval"); }
        }
        public void UpdateWeatherTypes()
        {
            WeatherTypes = new ObservableCollection<WeatherType>(Model.GetUpdatedWeatherTypes(SelectedParameter));
        }

        public void UdpateMedians()
        {
            if (SelectedParameter == WeatherType.ParameterEnum.Observation)
            {
                Medians = new ObservableCollection<WeatherType>(){
                    WeatherType.AveTempMedian,
                    WeatherType.MinTempMedian,
                    WeatherType.MaxTempMedian
                };
                MedianVisibility = Visibility.Visible;
            }
            else
            {
                Medians.Clear();
                MedianVisibility = Visibility.Collapsed;
            }
        }
        public void OnUpdateSelectedWeatherParameter()
        {
            Console.WriteLine("WeatherParameter " + SelectedParameter);
            UpdateDateTimeMinMax();
            UpdateWeatherTypes();
            UdpateMedians();
        }
        public void OnUpdateSelectedWeatherType()
        {
            Console.WriteLine("WeatherTypes " + SelectedWeatherTypes.Count);
            SelectedWeatherTypes.ForEach(selected =>
            {
                Console.WriteLine(selected.ToString());
            });
        }

        public void OnUpdateSelectedMedians()
        {
            Console.WriteLine("Medians " + SelectedMedians.Count);
            SelectedMedians.ForEach(selected =>
            {
                Console.WriteLine(selected.ToString());
            });
        }

        public override void UpdateDateTimeMinMax()
        {
            if (SelectedParameter == WeatherType.ParameterEnum.Observation)
            {
                DateTimeViewModel.UpdateDateTimeMinMax(DateTimeViewModel.DefaultDateTimeMin, DateTime.Now);
            }
            else
            {
                DateTimeViewModel.UpdateDateTimeMinMax(DateTimeViewModel.DefaultDateTimeMin,
                    DateTimeViewModel.DefaultDateTimeMax);
            }
        }
        public override void CreateDateTimeViewModel()
        {
            var dateTimeInputModel = Model.CreateDateTimeInputModel();
            DateTimeViewModel = new DateTimeViewModel(dateTimeInputModel);
            UpdateDateTimeMinMax();
        }

        public RelayCommand UpdateWeatherParameterCommand => new RelayCommand(() => OnUpdateSelectedWeatherParameter());
        public RelayCommand UpdateWeatherTypeCommand => new RelayCommand(() => OnUpdateSelectedWeatherType());
        public RelayCommand UpdateMedianCommand => new RelayCommand(() => OnUpdateSelectedMedians());

        public WeatherInputViewModel(IWeatherInputModel model)
        {
            _Model = model;
            Intervals = new ObservableCollection<Interval>(model.Intervals);
            SelectedInterval = Intervals.ToList().Find(interval => interval.Value == 60);
            UpdateWeatherTypes();
            UdpateMedians();
            CreateDateTimeViewModel();
        }
    }
}
