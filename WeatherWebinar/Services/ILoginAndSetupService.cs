using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace WeatherWebinar.Services
{
	public interface ILoginAndSetupService
	{
		Task<MobileServiceUser> Authenticate();
		//Task<string> GetDefaultCity();
	}
}
