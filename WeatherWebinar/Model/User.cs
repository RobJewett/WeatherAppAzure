using System;
using Newtonsoft.Json;

namespace WeatherWebinar.Model
{
	public class User
	{

		string _id;

		[JsonProperty("id")]
		public string Id
		{
			get
			{
				return _id;
			}
			set
			{
				_id = value;
			}
		}

		public string firstName { get; set; }
		public string lastName { get; set; }
		public string email { get; set; }
		public string defaultLocation { get; set; }

	}
}
