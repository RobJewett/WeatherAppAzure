using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Foundation;
using UIKit;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using WeatherWebinar.Services;
using HockeyApp.iOS;

namespace WeatherWebinar.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		string HockeyAppId_iOS = "9db4a72f06c74ee093f50aafce1969fd";
		private MobileServiceUser user;


		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB(43, 132, 211); //bar background
			UINavigationBar.Appearance.TintColor = UIColor.White; //Tint color of button items
			UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes()
			{
				Font = UIFont.FromName("HelveticaNeue-Light", 20f),
				TextColor = UIColor.White
			});

			Forms.Init();

#if DEBUG
			Xamarin.Calabash.Start();
#endif

			InitializeHockeyApp(HockeyAppId_iOS);

			LoadApplication(new App());

			var myButton = new UIButton();
			myButton.AccessibilityIdentifier = "MyButton";

			return base.FinishedLaunching(app, options);
		}

		public override void OnResignActivation(UIApplication application)
		{
			// Invoked when the application is about to move from active to inactive state.
			// This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
			// or when the user quits the application and it begins the transition to the background state.
			// Games should use this method to pause the game.
		}

		public override void DidEnterBackground(UIApplication application)
		{
			// Use this method to release shared resources, save user data, invalidate timers and store the application state.
			// If your application supports background exection this method is called instead of WillTerminate when the user quits.
		}

		public override void WillEnterForeground(UIApplication application)
		{
			// Called as part of the transiton from background to active state.
			// Here you can undo many of the changes made on entering the background.
		}

		public override void OnActivated(UIApplication application)
		{
			// Restart any tasks that were paused (or not yet started) while the application was inactive. 
			// If the application was previously in the background, optionally refresh the user interface.
		}

		public override void WillTerminate(UIApplication application)
		{
			// Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
		}

		void InitializeHockeyApp(string iOSHockeyAppID)
		{
			var manager = BITHockeyManager.SharedHockeyManager;
			manager.Configure(iOSHockeyAppID);
			manager.LogLevel = BITLogLevel.Debug;
			manager.StartManager();
			manager.Authenticator.AuthenticateInstallation();
			manager.UpdateManager.CheckForUpdate();
		}

	}
}
