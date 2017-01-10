// Helpers/Settings.cs
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using WeatherWebinar.Model;

namespace WeatherWebinar.Helpers
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public class Settings : INotifyPropertyChanged
	{

		static Settings settings;
		public static Settings Current
		{
			get { return settings ?? (settings = new Settings()); }
		}

		#region MOBILEAPPID & SERVICEAPPID & REDIRECTURI
		const string MobileAppURLKey = nameof(MobileAppURLKey);
		const string DefaultMobileAppUrl = "https://weatherwebinar.azurewebsites.net/";
		public string MobileAppURL
		{
			get { return AppSettings.GetValueOrDefault<string>(MobileAppURLKey, DefaultMobileAppUrl); }

			set { AppSettings.AddOrUpdateValue<string>(MobileAppURLKey, value); }
		}


		const string MobileAppAuthUrlKey = nameof(MobileAppAuthUrlKey);
		const string DefaultMobileAppAuthUrl = "https://weatherwebinar.azurewebsites.net/.auth/login/done";
		public string MobileAppAuthURL
		{
			get { return AppSettings.GetValueOrDefault<string>(MobileAppAuthUrlKey, DefaultMobileAppAuthUrl); }

			set { AppSettings.AddOrUpdateValue<string>(MobileAppAuthUrlKey, value); }
		}

		const string MobileAppIDKey = nameof(MobileAppIDKey);
		const string DefaultMobileAppID = "4b6b58a5-c6cb-409d-92cd-3b7a30879e94"; //weatherwebinarapp native app
		public string MobileAppID
		{
			get { return AppSettings.GetValueOrDefault<string>(MobileAppIDKey, DefaultMobileAppID); }

		}

		const string ServiceAppIDKey = nameof(ServiceAppIDKey);
		const string DefaultServiceAppID = "063502cf-d313-40f6-9f75-e59f515aa154"; //weatherwebinar app service 
		public string ServiceAppID
		{
			get { return AppSettings.GetValueOrDefault<string>(ServiceAppIDKey, DefaultServiceAppID); }

		}


		#endregion

		#region USER INFO

		private const string CurrentUserIdKey = nameof(CurrentUserIdKey);
		public const string DefaultCurrentUserId = "";

		public UserModel CurrentUser
		{
			get
			{
				string obj = AppSettings.GetValueOrDefault<string>(CurrentUserIdKey, DefaultCurrentUserId);
				if (obj == "null" || obj == "")
				{
					return new UserModel();
				}
				return JsonConvert.DeserializeObject<UserModel>(obj);
			}
			set
			{
				AppSettings.AddOrUpdateValue<string>(CurrentUserIdKey, JsonConvert.SerializeObject(value));
			}
		}

		private const string MobileServiceUserKey = nameof(MobileServiceUserKey);
		public MobileServiceUser CurrentMobileServiceUser
		{
			get
			{
				string obj = AppSettings.GetValueOrDefault<string>(MobileServiceUserKey, "");

				if (obj == "null" || obj == "")
				{
					return null;
				}
				return JsonConvert.DeserializeObject<MobileServiceUser>(obj);
			}
			set
			{
				AppSettings.AddOrUpdateValue<string>(MobileServiceUserKey, JsonConvert.SerializeObject(value));
			}

		}




		#endregion

		#region City Constants
		private const string IsImperialKey = nameof(IsImperialKey);
		private static readonly bool IsImperialDefault = true;

		public static bool IsImperial
		{
			get
			{
				return AppSettings.GetValueOrDefault(IsImperialKey, IsImperialDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(IsImperialKey, value);
			}
		}

		private const string UseCityKey = nameof(UseCityKey);
		private static readonly bool UseCityDefault = true;

		public static bool UseCity
		{
			get
			{
				return AppSettings.GetValueOrDefault(UseCityKey, UseCityDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(UseCityKey, value);
			}
		}

		private const string CityKey = nameof(CityKey);
		private static readonly string CityDefault = "San Francisco, CA";

		public string City
		{
			get
			{
				var j =  AppSettings.GetValueOrDefault(CityKey, CityDefault);
				return j;
			}
			set
			{
				AppSettings.AddOrUpdateValue(CityKey, value);
				OnPropertyChanged();
			}
		}

		#endregion

		#region Setting Constants

		private const string SettingsKey = "settings_key";
		private static readonly string SettingsDefault = string.Empty;
		#endregion

		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged([CallerMemberName]string name = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		#endregion



		public static string GeneralSettings
		{
			get
			{
				return AppSettings.GetValueOrDefault<string>(SettingsKey, SettingsDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue<string>(SettingsKey, value);
			}
		}

		private static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}
	}
}