using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace WeatherAndPower.Core
{
	public abstract class AbstractModel
	{
		// NotifyPropertyChanged is needed in order to be notified when a property changes, properties need to fire the event themselfs!
		// Take a look in how I did it above with Name and State, use snippets in VisualStudio to make the declaration of these a ease and not waist a lot of time...
		#region INotifyPropertyChanged implementation
		public event PropertyChangedEventHandler PropertyChanged;

		public void NotifyPropertyChanged(string propName)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
		}
		#endregion
	}
}
