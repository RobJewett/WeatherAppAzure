using System;
using Xamarin.Forms;
using WeatherWebinar.Model;
namespace WeatherWebinar
{
	public class ForecastDataTemplate : ImageCell
	{
		public ForecastDataTemplate()
		{
			this.SetBinding<ForecastListRoot>(TextProperty, m => m.DisplayTemp);
			this.SetBinding<ForecastListRoot>(ImageSourceProperty, m => m.DisplayIcon);
			//this.SetBinding<ForecastListRoot>(DetailProperty, m => m.DisplayDate);
			//this.SetValue(DetailProperty, "Detail");
		}
	}
}
