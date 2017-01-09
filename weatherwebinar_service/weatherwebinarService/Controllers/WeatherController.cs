using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using weatherwebinarService.WeatherModels;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using Newtonsoft.Json;

namespace weatherwebinarService.Controllers
{   [Authorize]
    [MobileAppController]
    public class WeatherController : ApiController
    {

        public enum Units
        {
            Imperial, Metric
        }

        const string WeatherCoordinatesUri = "http://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&units={2}&appid=4271ac29c66ce5ac5ed0eca94392adb8";
        const string WeatherCityUri = "http://api.openweathermap.org/data/2.5/weather?q={0}&units={1}&appid=4271ac29c66ce5ac5ed0eca94392adb8";
        const string ForecaseUri = "http://api.openweathermap.org/data/2.5/forecast/daily?id={0}&units={1}&cnt={2}&appid=4271ac29c66ce5ac5ed0eca94392adb8";

        static TimeSpan HttpTimeout = TimeSpan.FromSeconds(20);
        HttpClient Client = new HttpClient { Timeout = HttpTimeout };


        //[Authorize]
        [HttpGet, Route("api/getWeather/location")]
        // GET api/getWeather/location
        public async Task<WeatherRoot> GetWeatherLocation(double latitude, double longitude)
        {
            return await GetDataObjectFromAPI<WeatherRoot>(string.Format(WeatherCoordinatesUri, latitude, longitude, Units.Imperial.ToString().ToLower()));

        }

        
        [HttpGet, Route("api/getWeather/city")]
        // GET api/getWeather/city
        public async Task<WeatherRoot> GetWeatherCity(string city)
        {
            return await GetDataObjectFromAPI<WeatherRoot>(string.Format(WeatherCityUri, city, Units.Imperial.ToString().ToLower()));

        }

        //[Authorize]
        [HttpGet, Route("api/getWeather/forecast")]
        // GET api/getWeather/forecast
        public async Task<WeatherForecastRoot> GetForecast(int id, int numDays)
        {
            return await GetDataObjectFromAPI<WeatherForecastRoot>(string.Format(ForecaseUri, id, Units.Imperial.ToString().ToLower(), numDays));
        }

        #region ACTUAL Api Call
        async Task<T> GetDataObjectFromAPI<T>(string apiUrl)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var json = await Client.GetStringAsync(apiUrl);

                    if (string.IsNullOrWhiteSpace(json))
                        return default(T);

                    var obj = JsonConvert.DeserializeObject<T>(json);

                    return obj;
                }
                catch (Exception e)
                {
                    return default(T);
                }

            });
        }
        #endregion
        

    }
}
