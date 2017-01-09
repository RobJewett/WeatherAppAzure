using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using UIKit;
using WeatherWebinar.Helpers;
using WeatherWebinar.iOS;
using WeatherWebinar.Services;


[assembly: Xamarin.Forms.Dependency(typeof(LoginAndSetupiOS))]
namespace WeatherWebinar.iOS
{
	public class LoginAndSetupiOS:ILoginAndSetupService
	{
		public MobileServiceUser user;
		public async Task<MobileServiceUser> Authenticate()
		{
			var success = false;
			var message = string.Empty;

			try
			{
				// Sign in with Azure AD login using a server-managed flow.
				if (user == null)
				{
					user = await AzureService.Instance.MobileService
						.LoginAsync(UIApplication.SharedApplication.KeyWindow.RootViewController,
						MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory);
					if (user != null)
					{
						//message = string.Format("You are now signed-in as {0}.", user.UserId);
						success = true;
						Settings.Current.CurrentMobileServiceUser = user;
						App.LoggedIn = true;
					}
				}
			}
			catch (Exception ex)
			{
				message = ex.Message;
			}

			// Display the failure message.
			if (!string.IsNullOrEmpty(message))
			{
				UIAlertView avAlert = new UIAlertView("Uh-Oh!", message, null, "OK", null);
				avAlert.Show();
			}
			return user;
		}
	}
}
