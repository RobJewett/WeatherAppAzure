using System;
using WeatherWebinar.Helpers;
using WeatherWebinar.Model;
using WeatherWebinar.Services;
using Xamarin.Forms;

namespace WeatherWebinar
{
	public class LoginPage : ContentPage
	{
		Button GetStartedButton;
		Entry CityEntry;
		Button NextButton;

		public LoginPage()
		{
			NavigationPage.SetHasBackButton(this, false);
			Title = "Weather App";

			BackgroundColor = Color.FromRgb(43, 132, 211);

			NextButton = new Button { 
				Text = "Finished", IsVisible = false, 
				VerticalOptions = LayoutOptions.Center, 
				BorderColor = Color.White,
				BorderWidth = 1,
				TextColor = Color.White};

			CityEntry = new Entry { Placeholder = "Enter Your Default City", IsVisible = false };

			var img = ImageSource.FromFile("weathersunicon.png");

			var image = new Image();
			image.Source = img;
			image.Scale = 0.5f;

			GetStartedButton = new Button
			{
				Text = "Get Started",
				VerticalOptions = LayoutOptions.Center,
				BorderColor = Color.White,
				BorderWidth = 1,
				TextColor = Color.White,
			};

			GetStartedButton.Clicked += async (sender, e) =>
			{
				var tempuser = await DependencyService.Get<ILoginAndSetupService>().Authenticate();
				if (tempuser != null)
				{
					// NOTE: This is NOT best practise, you should NEVER store user information 
					// in NSUserDefaults/SharedPreferences, as this contains the API Token
					// furthermore, we aren't taking note of the Token Expiry either!

					//Settings.Current.CurrentMobileServiceUser = tempuser;
					//App.LoggedIn = true;
					await AzureService.Instance.Initialize();
					var user = await AzureService.Instance.GetCurrentUser();
					if (user.Count != 0)
					{
						await DisplayAlert("Welcome!", $"Welcome Back {user[0].firstName}!", "Ok");
						Settings.Current.City = user[0].defaultLocation;
						await Navigation.PopAsync();
					}
					else {
						NextButton.IsVisible = true;
						CityEntry.IsVisible = true;
						GetStartedButton.IsVisible = false;
					}
				}
			};

			NextButton.Clicked += async (sender, e) =>
			{
				if (string.IsNullOrEmpty(CityEntry.Text))
				{
					await DisplayAlert("Error", "Please Enter A Default City", "OK");
				}
				else {
					//lets save our user to azure
					var success = await AzureService.Instance.SaveUserAsync(new Model.User { defaultLocation = CityEntry.Text });
					if (success)
					{
						Settings.Current.City = CityEntry.Text;
						await Navigation.PopAsync();
					}
				}
			};

			Content = new StackLayout
			{
				Children = {
					image,
					GetStartedButton,
					CityEntry,
					NextButton
				},
				//VerticalOptions = LayoutOptions.Center, 
				Padding = 30
			};
		}

	}
}

