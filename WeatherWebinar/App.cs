//using MyWeather.View;
//using MyWeather.ViewModels;
using Xamarin.Forms;
using static System.Diagnostics.Debug;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using WeatherWebinar.Helpers;
using WeatherWebinar.Services;
using System;

namespace WeatherWebinar
{
	public class App : Application
	{
		public static bool LoggedIn = false;
		public App()
		{
			var tabs = new TabbedPage
			{
				Title = "My Weather",
				BindingContext = new WeatherViewModel(),
				Children =
				{
					new WeatherView(),
					new ForecastView(),
					new AccountView()
				}
			};

			MainPage = new NavigationPage(tabs)
			{
				BarBackgroundColor = Color.FromHex("3498db"),
				BarTextColor = Color.White
			};

			if (Settings.Current.CurrentMobileServiceUser != null)
			{
				AzureService.Instance.Initialize();
			}
			else {
				MainPage.Navigation.PushAsync(new LoginPage());
			}


		}


		protected override void OnStart()
		{
			base.OnStart();
			WriteLine("Application OnStart");
		}

		protected override void OnSleep()
		{
			base.OnSleep();
			WriteLine("Application OnSleep");
		}

		protected override void OnResume()
		{
			base.OnResume();
			WriteLine("Application OnResume");
		}

	}
}

