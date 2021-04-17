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
        #region Properties
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

        //private ECity _SelectedCity { get; set; } = (ECity)0x01;
        public ECity SelectedCity
        {
            get { return Model.Preference.CityEnum; }
            set { Model.Preference.CityEnum = value; NotifyPropertyChanged("SelectedCity"); }
        }

        public ObservableCollection<WeatherType.ParameterEnum> WeatherParameters
        {
            get { return Model.WeatherParameters; }
        }

        //private WeatherType.ParameterEnum _SelectedParameter { get; set; } = WeatherType.ParameterEnum.Observation;
        public WeatherType.ParameterEnum SelectedParameter
        {
            get { return Model.Preference.WeatherParameter; }
            set
            {
                Model.Preference.WeatherParameter = value;
                NotifyPropertyChanged("SelectedParameter");
                OnUpdateSelectedWeatherParameter();
            }
        }

        //private ObservableCollection<WeatherType> _WeatherTypes { get; set; }
        public ObservableCollection<WeatherType> WeatherTypes
        {
            get { return Model.WeatherTypes; }
            //set { _WeatherTypes = value; NotifyPropertyChanged("WeatherTypes"); }
        }

        public List<WeatherType> SelectedWeatherTypes { get; set; } = new List<WeatherType>();

        private Visibility _MedianVisibility { get; set; } = Visibility.Visible;
        public Visibility MedianVisibility
        {
            get { return _MedianVisibility; }
            set { _MedianVisibility = value; NotifyPropertyChanged("MedianVisibility"); }
        }
        //public ObservableCollection<WeatherType> _Medians { get; set; } = new ObservableCollection<WeatherType>();

        public ObservableCollection<WeatherType> Medians
        {
            get { return Model.Mideans; }
        }

        public List<WeatherType> SelectedMedians { get; set; } = new List<WeatherType>();

        public ObservableCollection<ECity> Cities
        {
            get { return Model.Cities; }
        }


        private ObservableCollection<Interval> _Intervals { get; set; } = new ObservableCollection<Interval>();

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
        #endregion

        public void UdpateMedians()
        {
            Model.Preference.Medians.Clear();
            if (Model.Mideans.Count != 0)
            {
                Model.Mideans.Clear();
            }
            if (SelectedParameter == WeatherType.ParameterEnum.Observation)
            {
                Model.Mideans.Add(WeatherType.AveTempMedian);
                Model.Mideans.Add(WeatherType.MinTempMedian);
                Model.Mideans.Add(WeatherType.MaxTempMedian);
                MedianVisibility = Visibility.Visible;
            }
            else
            {
                MedianVisibility = Visibility.Collapsed;
            }
        }
        public void OnUpdateSelectedWeatherParameter()
        {
            Console.WriteLine("WeatherParameter " + SelectedParameter);
            UpdateDateTimeMinMax();
            Model.UpdateWeatherTypes();
            UdpateMedians();
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

        public WeatherInputViewModel(IWeatherInputModel model)
        {
            _Model = model;
            Intervals = new ObservableCollection<Interval>(model.Intervals);
            SelectedInterval = Intervals.ToList().Find(interval => interval.Value == 60);
            Model.UpdateWeatherTypes();
            UdpateMedians();
            CreateDateTimeViewModel();
        }
    }
}
