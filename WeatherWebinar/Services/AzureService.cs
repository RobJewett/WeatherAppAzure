using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherWebinar.Model;
using Newtonsoft.Json;
using WeatherWebinar.Helpers;

using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace WeatherWebinar.Services
{
	public enum Units
	{
		Imperial,
		Metric
	}

	public class AzureService
	{
		public MobileServiceClient MobileService { get; set; }
		IMobileServiceSyncTable<User> UserTable;

		const string DBPath = "accounts.db";

		public AzureService()
		{
			MobileService = new MobileServiceClient("https://weatherwebinar.azurewebsites.net");
		}

		public async Task Initialize()
		{
			if (MobileService?.SyncContext?.IsInitialized ?? false)
				return;

			if (Settings.Current.CurrentMobileServiceUser != null)
			{
				MobileService.CurrentUser = Settings.Current.CurrentMobileServiceUser;
			}

			var path = "syncstore.db";
			path = Path.Combine(MobileServiceClient.DefaultDatabasePath, path);

			//setup our local sqlite store and intialize our table
			var store = new MobileServiceSQLiteStore(path);
			store.DefineTable<User>();

			await MobileService.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());

			//Get our sync table that will call out to azure
			UserTable = MobileService.GetSyncTable<User>();
			await SyncUserAsync();
		}

		#region WEATHER 

		public async Task<WeatherRoot> GetWeather(double latitude, double longitude, Units units = Units.Imperial)
		{
			try
			{
				var arguments = new Dictionary<string, string>
				{
					{("latitude"),($"{latitude}")},
					{("longitude"),($"{longitude}")}
				};

				var res = await MobileService.InvokeApiAsync<WeatherRoot>("getWeather/location",HttpMethod.Get,arguments);

				return res;
			}
			catch (Exception e)
			{
				HockeyappHelpers.Report(e);
				return null;
			}
		}

		public async Task<WeatherRoot> GetWeather(string city, Units units = Units.Imperial)
		{
			try
			{
				var arguments = new Dictionary<string, string>
				{
					{("city"),($"{city}")}
				};

				var res = await MobileService.InvokeApiAsync<WeatherRoot>("getWeather/city",HttpMethod.Get,arguments);

				return res;
			}
			catch (Exception e)
			{
				HockeyappHelpers.Report(e);
				return null;
			}
		}

		public async Task<WeatherForecastRoot> GetForecast(int id, Units units = Units.Imperial)
		{
			return await Task.Run(async () =>
			{
				try
				{
					var arguments = new Dictionary<string, string>
				{
					{("id"),($"{id}")},
					{("numDays"),("16")}
				};

					var res = await MobileService.InvokeApiAsync<WeatherForecastRoot>("getWeather/forecast", HttpMethod.Get, arguments);

					return res;
				}
				catch (Exception e)
				{
					HockeyappHelpers.Report(e);
					return null;
				}
			});
		}

		#endregion

		#region USER SYNCING

		public async Task SyncUserAsync()
		{
			ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

			try
			{
				await MobileService.SyncContext.PushAsync();

				await this.UserTable.PullAsync(
					//The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
					//Use a different query name for each unique query in your program
					null,
					this.UserTable.CreateQuery());
			}
			catch (MobileServiceInvalidOperationException ex)
			{
				if (ex.Response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
				{
					//we have to authenticate again
					await DependencyService.Get<ILoginAndSetupService>().Authenticate();
					return;
				}
			}
			catch (MobileServicePushFailedException exc)
			{
				
				if (exc.PushResult != null)
				{
					syncErrors = exc.PushResult.Errors;
				}
			}

			// Simple error/conflict handling. A real application would handle the various errors like network conditions,
			// server conflicts and others via the IMobileServiceSyncHandler.
			if (syncErrors != null)
			{
				foreach (var error in syncErrors)
				{
					if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
					{
						//Update failed, reverting to server's copy.
						await error.CancelAndUpdateItemAsync(error.Result);
					}
					else
					{
						// Discard local change.
						await error.CancelAndDiscardItemAsync();
					}

					Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
				}
			}
		}


		public async Task<List<User>> GetUser()
		{
			await UserTable.PurgeAsync();
			await SyncUserAsync();
			return await UserTable.ToListAsync();
		}

		public async Task<List<User>> GetCurrentUser()
		{
			return await UserTable.ToListAsync();
		}

		public async Task<bool> SaveUserAsync(User _user)
		{
			// if the email field is null, create a new user in azure
			await Initialize();
			try
			{
				if (_user.email == null)
				{
					await UserTable.InsertAsync(_user);
				}
				// else, the user is updating their preferred default location
				else
				{
					await UserTable.UpdateAsync(_user);
				}
				await SyncUserAsync();
				return true;
			}
			catch(Exception e)
			{
				System.Diagnostics.Debug.WriteLine(e.Message);
				return false;
			}
		}


		#endregion

		static readonly AzureService instance = new AzureService();

		/// <summary>
		/// Gets the instance of the Azure Web Service
		/// </summary>
		public static AzureService Instance
		{
			get
			{
				return instance;
			}
		}

	}
}
