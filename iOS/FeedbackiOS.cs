using System;
using System.Threading.Tasks;
using WeatherWebinar.Helpers;
using WeatherWebinar.Services;
using WeatherWebinar.iOS;
using HockeyApp.iOS;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(FeedbackiOS))]
namespace WeatherWebinar.iOS
{
	public class FeedbackiOS:IHockeyAppFeedbackService
	{

		public async Task GiveFeedback()
		{
			var feedbackManager = BITHockeyManager.SharedHockeyManager.FeedbackManager;

			var alert = UIAlertController.Create("Give Feedback", "Provide Feedback to the Developers", UIAlertControllerStyle.ActionSheet);
			alert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));
			alert.AddAction(UIAlertAction.Create("Review Existing Feedback", UIAlertActionStyle.Default, (obj) => feedbackManager.ShowFeedbackListView()));
			alert.AddAction(UIAlertAction.Create("Submit New Feedback", UIAlertActionStyle.Default, (obj) => feedbackManager.ShowFeedbackComposeView()));

			var window = UIApplication.SharedApplication.KeyWindow;
			var vc = window.RootViewController;
			while (vc.PresentedViewController != null)
			{
				vc = vc.PresentedViewController;
			}

			await vc.PresentViewControllerAsync(alert, true);
		}
	}
}
