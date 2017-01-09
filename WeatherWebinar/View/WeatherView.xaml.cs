using System;
using System.Collections.Generic;
using WeatherWebinar.Helpers;
using Xamarin.Forms;

namespace WeatherWebinar
{
	public partial class WeatherView : ContentPage
	{
		public WeatherView()
		{
			InitializeComponent();

			if (Device.OS == TargetPlatform.iOS)
				Icon = new FileImageSource { File = "tab1.png" };

			var feedbackToolBarItem = new ToolbarItem
			{
				Icon = "Add",
				AutomationId = "FeedbackButton"
			};
			feedbackToolBarItem.SetBinding(ToolbarItem.CommandProperty, "FeedbackButtonTapped");
			ToolbarItems.Add(feedbackToolBarItem);

			InitializeAutomationIds();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			HockeyappHelpers.TrackEvent("Weather Page Appeared");
		}

		void InitializeAutomationIds()
		{
			TempLabel.AutomationId = "TempLabel";
			UseGPSSwitch.AutomationId = "GPSSwitch";
			LocationEntry.AutomationId = "LocationEntry";
			ConditionLabel.AutomationId = "ConditionLabel";
			GetWeatherButton.AutomationId = "GetWeatherBtn";
			GetWeatherActivityIndicator.AutomationId = "GetWeathActivityIndicator";
		}
	}
}
