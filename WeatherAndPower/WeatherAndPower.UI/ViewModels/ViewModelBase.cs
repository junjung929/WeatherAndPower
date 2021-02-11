using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAndPower.UI
{
	/*
	 * ViewModel base class that implements all the necessary databinding boilerplate
	 */
	public abstract class ViewModelBase : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged implementation
		public event PropertyChangedEventHandler PropertyChanged;

		public void NotifyPropertyChanged(string propName)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
		}
		#endregion

		[Conditional("DEBUG")]
		private void checkIfPropertyNameExists(String propertyName)
		{
			Type type = this.GetType();
			Debug.Assert(
			  type.GetProperty(propertyName) != null,
			  propertyName + "property does not exist on object of type : " + type.FullName);
		}

		public bool SetProperty<T>(ref T field, T value, string propertyName)
		{
			if (!EqualityComparer<T>.Default.Equals(field, value))
			{
				field = value;

				checkIfPropertyNameExists(propertyName);

				this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
