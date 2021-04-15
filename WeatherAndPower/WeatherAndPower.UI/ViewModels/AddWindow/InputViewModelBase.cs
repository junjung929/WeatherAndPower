using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public InputViewModelBase()
        {
        }
    }
}
