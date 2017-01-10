using System;
using System.Collections.Generic;
using WeatherWebinar.Helpers;
using Xamarin.Forms;

namespace WeatherWebinar
{
	public partial class ForecastView : ContentPage
	{
		ListView _listViewWeather;

		public ForecastView()
		{
			if (Device.OS == TargetPlatform.iOS)
				Icon = new FileImageSource { File = "tab2.png" };

			Title = "Forecast";
			_listViewWeather = new ListView
			{ 
				ItemTemplate = new DataTemplate(typeof(ForecastDataTemplate)),
				SeparatorColor = Color.Transparent
			};
			_listViewWeather.SetBinding<WeatherViewModel>(ListView.ItemsSourceProperty, vm => vm.Forecast.Items);
			_listViewWeather.ItemTapped += (sender, args) => _listViewWeather.SelectedItem = null;

#if DEBUG
			var crashButtonToolBarItem = new ToolbarItem
			{
				Icon = "Crash",
				AutomationId = "CrashButton"
			};
			crashButtonToolBarItem.SetBinding(ToolbarItem.CommandProperty, "CrashButtonTapped");
			ToolbarItems.Add(crashButtonToolBarItem);
#endif
			Content = _listViewWeather;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			HockeyappHelpers.TrackEvent("Forecast Page Appeared");
		}
	}
}
