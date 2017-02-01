using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using HockeyApp.Android;
using HockeyApp.Android.Metrics;
using Plugin.Permissions;
using Xamarin.Forms.Platform.Android;
using System.Reflection.Emit;

[assembly: MetaData("net.hockeyapp.android.appIdentifier", Value = "aa8b2cffe4674bea832a7d5ff89e111e")]
namespace WeatherWebinar.Droid
{
	[Activity(Label = "WeatherWebinar.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : FormsAppCompatActivity
	{
		string HockeyAppId_Droid = "aa8b2cffe4674bea832a7d5ff89e111e";

		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);
			InitializeHockeyApp(HockeyAppId_Droid);


			LoadApplication(new App());
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		{
			PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}

		protected override void OnResume()
		{
			base.OnResume();
			//Tracking.StartUsage(this);
		}

		protected override void OnPause()
		{
			base.OnPause();
			//Tracking.StopUsage(this);
		}

		void InitializeHockeyApp(string hockeyAppID)
		{
			CrashManager.Register(this, hockeyAppID);
			UpdateManager.Register(this, hockeyAppID, true);
			FeedbackManager.Register(this, hockeyAppID, null);
			MetricsManager.Register(Application);
		}

	}
}
