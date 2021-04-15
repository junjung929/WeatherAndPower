using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.UI.ViewModels.AddWindow
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
