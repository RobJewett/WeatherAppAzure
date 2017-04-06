using System;
using System.Collections.Generic;
using WeatherWebinar.Services;
using Xamarin.Forms;
using System.Runtime.CompilerServices;


namespace WeatherWebinar.Helpers
{
	public static class HockeyappHelpers
	{
		enum _pathType { Windows, Linux };

		public static void TrackEvent(string eventName)
		{
			HockeyApp.MetricsManager.TrackEvent(eventName);
			 
		}



		public static void TrackEvent(string eventName, Dictionary<string, string> properties, Dictionary<string, double> measurements)
		{
			HockeyApp.MetricsManager.TrackEvent(eventName, properties, measurements);
		}

		#region REPORT
		/// <summary>
		/// Reports a caught exception to Hockeyapp
		/// </summary>
		public static void Report(Exception exception, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string callerMembername = "")
		{
			var fileName = GetFileNameFromFilePath(filePath);

			var errorReport = $"Error: {exception.Message} ";
			errorReport += $"Line Number: {lineNumber} ";
			errorReport += $"Caller Name: {callerMembername} ";
			errorReport += $"File Name: {fileName}";

			TrackEvent(errorReport);
		}

		static string GetFileNameFromFilePath(string filePath)
		{
			string fileName;
			_pathType pathType;

			var directorySeparator = new Dictionary<_pathType, string>
			{
				{ _pathType.Linux, "/" },
				{ _pathType.Windows, @"\" }
			};

			pathType = filePath.Contains(directorySeparator[_pathType.Linux]) ? _pathType.Linux : _pathType.Windows;

			while (true)
			{
				if (!(filePath.Contains(directorySeparator[pathType])))
				{
					fileName = filePath;
					break;
				}

				var indexOfDirectorySeparator = filePath.IndexOf(directorySeparator[pathType], StringComparison.Ordinal);
				var newStringStartIndex = indexOfDirectorySeparator + 1;

				filePath = filePath.Substring(newStringStartIndex, filePath.Length - newStringStartIndex);
			}

			return fileName;

		}

		#endregion

	}
}
