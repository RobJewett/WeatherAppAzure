using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace UITests
{
	[TestFixture(Platform.Android)]
	[TestFixture(Platform.iOS)]
	public class Tests
	{
		IApp app;
		Platform platform;

		public Tests(Platform platform)
		{
			this.platform = platform;
		}

		[SetUp]
		public void BeforeEachTest()
		{
			app = AppInitializer.StartApp(platform);
		}

		[Test]
		public void WeatherTest()
		{
			app.Screenshot("On the Main Page");

			app.ClearText("LocationEntry");

			app.EnterText("LocationEntry","Toronto");
			app.Screenshot("Entering Toronto as our Location");

			app.Screenshot("Pressing Get Weather button");

			app.Tap("GetWeatherBtn");

			app.WaitForElement(x=>x.Marked("ConditionLabel"),"Timed out waiting for weather data to return");

			app.Screenshot("Retreieved weather data");

			app.Tap("Forecast"); 

			app.Screenshot("Viewing Forecast");
		}
	}
}
