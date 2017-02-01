using System.Threading.Tasks;
using Xamarin.Forms;
using HockeyApp.Android;

using WeatherWebinar.Services;
using MyWeather.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(FeedbackDroid))]
namespace MyWeather.Droid
{
	public class FeedbackDroid : IHockeyAppFeedbackService
	{
		public async Task GiveFeedback()
		{
			await Task.Run(() => FeedbackManager.ShowFeedbackActivity(Forms.Context));
		}
	}
}
