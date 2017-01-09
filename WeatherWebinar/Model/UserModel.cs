using System;
namespace WeatherWebinar.Model
{
	public class UserModel
	{
		public string firstName { get; set; }
		public string lastName { get; set; }
		public string email { get; set; }
		public string AuthToken { get; set; }
		public DateTimeOffset authExpiry { get; set; }
		public string defaultLocation { get; set; }

	}
}
