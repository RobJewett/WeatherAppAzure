using System;
using System.Collections.Generic;
using WeatherWebinar.Helpers;
using Xamarin.Forms;

namespace WeatherWebinar
{
	public partial class ForecastView : ContentPage
	{
		public ForecastView()
		{
			InitializeComponent();
			if (Device.OS == TargetPlatform.iOS)
				Icon = new FileImageSource { File = "tab2.png" };
			ListViewWeather.ItemTapped += (sender, args) => ListViewWeather.SelectedItem = null;

#if DEBUG
			var crashButtonToolBarItem = new ToolbarItem
			{
				Icon = "Crash",
				AutomationId = "CrashButton"
			};
			crashButtonToolBarItem.SetBinding(ToolbarItem.CommandProperty, "CrashButtonTapped");
			ToolbarItems.Add(crashButtonToolBarItem);
#endif
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			HockeyappHelpers.TrackEvent("Forecast Page Appeared");
		}
	}
}
