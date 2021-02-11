using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAndPower.Contracts;

namespace WeatherAndPower.Core
{
    public class PlaceholderModel : IPlaceholderModel
    {
		private int _PlaceholderProperty = 0;
        public int PlaceholderProperty
		{
			get {
				return _PlaceholderProperty;
			}
			private set {
				if (_PlaceholderProperty != value) {
					_PlaceholderProperty = value;
				}
			}
		}

		public void PlaceholderAction()
		{
			PlaceholderProperty++;
		}
	}
}
