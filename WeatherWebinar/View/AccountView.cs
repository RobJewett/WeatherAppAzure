using System;
using WeatherWebinar.Services;
using Xamarin.Forms;
using System.Diagnostics;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.Generic;
using System.Linq;
using WeatherWebinar.Helpers;
using System.Threading.Tasks;

namespace WeatherWebinar
{
	public class AccountView : ContentPage
	{
		Label Name;
		Label Email;
		Entry DefaultLocation;
		Model.User _currentuser;

		public AccountView()
		{
		    
			if (Device.OS == TargetPlatform.iOS)
				Icon = new FileImageSource { File = "tab3.png" };

			Title = "Profile";

			Name = new Label();
			Email = new Label();
			DefaultLocation = new Entry();

			Button ChangeDefaultLocationButton = new Button { Text = "Change Location"};
			ChangeDefaultLocationButton.Clicked += async (sender, e) =>
			{
				var yesNo = await DisplayAlert("Update Location","Are you sure you want to change your location?","Yes","No");

				if(yesNo)
				{
					if (DefaultLocation.Text != _currentuser.defaultLocation)
					{
						_currentuser.defaultLocation = DefaultLocation.Text;

						var success = await AzureService.Instance.SaveUserAsync(_currentuser);
						if (!success)
						{
							await DisplayAlert("Error!", "Error changing your location", "OK");
						}
						else {
							await DisplayAlert("Success!", $"You have changed your default location to {DefaultLocation.Text}", "OK");
							Settings.Current.City = DefaultLocation.Text;
						}
					}
					else {
						await DisplayAlert("Same Location", "You didn't change the location!", "OK");
					}
				}
			};


			Content = new StackLayout
			{
				Children = {
					new Label{Text = "Name:", FontAttributes = FontAttributes.Bold},
					Name,
					new Label{Text = "Email:", FontAttributes = FontAttributes.Bold},
					Email,
					new Label{Text = "Default City:", FontAttributes = FontAttributes.Bold},
					DefaultLocation,
					ChangeDefaultLocationButton
				},
				Padding = 30
			};
		}

		protected async override void OnAppearing()
		{
			var user = await AzureService.Instance.GetCurrentUser();
			_currentuser = user.FirstOrDefault();
			Name.Text = _currentuser.firstName + " " + _currentuser.lastName;
			Email.Text = _currentuser.email;
			DefaultLocation.Text = _currentuser.defaultLocation;
		}
	}
}

