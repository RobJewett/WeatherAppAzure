using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

using Plugin.Geolocator;

using WeatherWebinar.Model;
using WeatherWebinar.Helpers;
using WeatherWebinar.Services;
using WeatherWebinar.Services;

namespace WeatherWebinar
{
	public class WeatherViewModel : INotifyPropertyChanged
	{
		const string _errorMessage = "Unable to get Weather";


		string location = Settings.Current.City;
		public string Location
		{
			get { return location; }
			set
			{
				location = value;
				OnPropertyChanged();
			}
		}

		bool useGPS;
		public bool UseGPS
		{
			get { return useGPS; }
			set
			{
				HockeyappHelpers.TrackEvent("GPS Switch Toggled",
					new Dictionary<string, string> { { "Use GPS Value", value.ToString() } },
					null);

				useGPS = value;
				OnPropertyChanged();
			}
		}




		bool isImperial = Settings.IsImperial;
		public bool IsImperial
		{
			get { return isImperial; }
			set
			{
				isImperial = value;
				OnPropertyChanged();
			}
		}



		string temp = string.Empty;
		public string Temp
		{
			get { return temp; }
			set { temp = value; OnPropertyChanged(); }
		}

		string condition = string.Empty;
		public string Condition
		{
			get { return condition; }
			set { condition = value; OnPropertyChanged(); }
		}


		bool isBusy = false;
		public bool IsBusy
		{
			get { return isBusy; }
			set { isBusy = value; OnPropertyChanged(); }
		}

		WeatherForecastRoot forecast;
		public WeatherForecastRoot Forecast
		{
			get { return forecast; }
			set { forecast = value; OnPropertyChanged(); }
		}


		ICommand getWeather;
		public ICommand GetWeatherCommand =>
				getWeather ??
		(getWeather = new Command(async () =>
		{
			await ExecuteGetWeatherCommand();
		}));


		ICommand crashButtonTapped;
		public ICommand CrashButtonTapped =>
				crashButtonTapped ??
		(crashButtonTapped = new Command(() =>
		{
			ExecuteCrashButtonCommand();
		}));


		ICommand feedbackButtonTapped;
		public ICommand FeedbackButtonTapped =>
				feedbackButtonTapped ??
		(feedbackButtonTapped = new Command(() =>
		{
			ExecuteFeedbackButtonCommand();
		}));


		ICommand changeDefaultLocation;
		public ICommand ChangeDefaultLocation =>
			changeDefaultLocation ??
		(changeDefaultLocation = new Command(async() =>
		{
			//await ExecuteChangeLocationCommand();
		}));

		//private async Task ExecuteChangeLocationCommand()
		//{
		//	var yesNo = await DisplayAlert("Update Location", "Are you sure you want to change your location?", "Yes", "No");

		//	if (yesNo)
		//	{
		//		if (DefaultLocation.Text != _currentuser.defaultLocation)
		//		{
		//			_currentuser.defaultLocation = DefaultLocation.Text;

		//			var success = await AzureService.Instance.SaveUserAsync(_currentuser);
		//			if (!success)
		//			{
		//				await DisplayAlert("Error!", "Error changing your location", "OK");
		//			}
		//			else {
		//				await DisplayAlert("Success!", $"You have changed your default location to {DefaultLocation.Text}", "OK");
		//				Settings.Current.City = DefaultLocation.Text;
		//			}
		//		}
		//		else {
		//			await DisplayAlert("Same Location", "You didn't change the location!", "OK");
		//		}
		//	}
		//}

		private void ExecuteCrashButtonCommand()
		{
			HockeyappHelpers.TrackEvent("Crash Button Tapped");
			throw new Exception("Crash Button Tapped");
		}

		private void ExecuteFeedbackButtonCommand()
		{
			HockeyappHelpers.TrackEvent("Feedback Button Tapped");
			DependencyService.Get<IHockeyAppFeedbackService>()?.GiveFeedback();
		}

		private async Task ExecuteGetWeatherCommand()
		{
			if (IsBusy)
				return;

			IsBusy = true;
			try
			{
				WeatherRoot weatherRoot = null;
				var units = IsImperial ? Units.Imperial : Units.Metric;


				if (UseGPS)
				{

					var gps = await CrossGeolocator.Current.GetPositionAsync(10000);
					weatherRoot = await AzureService.Instance.GetWeather(gps.Latitude, gps.Longitude, units);
				}
				else
				{
					//Get weather by city
					weatherRoot = await AzureService.Instance.GetWeather(Location.Trim(), units);
				}


				//Get forecast based on cityId
				//Forecast = await AzureService.Instance.GetForecast(weatherRoot.CityId, units);

				var unit = IsImperial ? "F" : "C";
				Temp = $"Temp: {weatherRoot?.MainWeather?.Temperature ?? 0}°{unit}";
				Condition = $"{weatherRoot?.Name}: {weatherRoot?.Weather?[0]?.Description ?? string.Empty}";

			}
			catch (Exception ex)
			{
				Temp = _errorMessage;
				HockeyappHelpers.Report(ex);
			}
			finally
			{
				IsBusy = false;
				TrackGetWeatherEvent();
			}
		}

		void TrackGetWeatherEvent()
		{
			var eventDictionaryHockeyApp = new Dictionary<string, string>
			{
				{"Use GPS Enabled", UseGPS.ToString()}
			};

			try
			{
				if (!Temp.Contains(_errorMessage))
				{

					var locationCityName = UseGPS
						? Condition?.Substring(0, Condition.IndexOf(":", StringComparison.Ordinal))
						: Location?.Substring(0, Location.IndexOf(",", StringComparison.Ordinal));

					eventDictionaryHockeyApp.Add("Location", locationCityName);
				}
			}
			catch (Exception ex)
			{
				HockeyappHelpers.Report(ex);
			}
			finally
			{
				HockeyappHelpers.TrackEvent("Weather Button Tapped", eventDictionaryHockeyApp, null);
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged([CallerMemberName]string name = "")
		{
			var handle = PropertyChanged;
			handle?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}
