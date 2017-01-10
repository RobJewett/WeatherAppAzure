using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using WeatherWebinar.Helpers;
using WeatherWebinar.Droid;
using WeatherWebinar.Services;
using Android.App;

[assembly: Xamarin.Forms.Dependency(typeof(LoginAndSetupDroid))]
namespace WeatherWebinar.Droid
{
	public class LoginAndSetupDroid : ILoginAndSetupService
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
											 .LoginAsync(Xamarin.Forms.Forms.Context, MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory);             
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
				AlertDialog.Builder alert = new AlertDialog.Builder(Xamarin.Forms.Forms.Context, 0)
											.SetTitle("Uh-Oh!")
											.SetMessage(message);
				alert.Show();
			}
			return user;
		}
	}
}
