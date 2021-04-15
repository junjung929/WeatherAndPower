using System;

namespace WeatherAndPower.UI
{
    public class InputViewModelBase : ViewModelBase
    {
        public DateTimeViewModel DateTimeViewModel { get; set; }

        public virtual void OnDateTimeUpdated(string dateTime)
        {
            throw new NotImplementedException();
        }

        public virtual void UpdateDateTimeMinMax()
        {
            throw new NotImplementedException();
        }

        public virtual void CreateDateTimeViewModel()
        {
            throw new NotImplementedException();
        }
        public InputViewModelBase()
        {
        }
    }
}
