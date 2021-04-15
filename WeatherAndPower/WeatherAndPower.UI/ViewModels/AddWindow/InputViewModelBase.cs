namespace WeatherAndPower.UI
{
    public class InputViewModelBase : ViewModelBase
    {
        public DateTimeViewModel DateTimeViewModel { get; set; }

        public virtual void OnDateTimeUpdated(string dateTime)
        {
        }

        public virtual void UpdateDateTimeMinMax()
        {

        }

        public virtual void CreateDateTimeViewModel()
        {

        }
        public InputViewModelBase()
        {
        }
    }
}
