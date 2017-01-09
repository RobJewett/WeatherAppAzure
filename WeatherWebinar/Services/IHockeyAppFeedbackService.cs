using System;
using System.Threading.Tasks;

namespace WeatherWebinar.Services
{
	public interface IHockeyAppFeedbackService
	{
		Task GiveFeedback();
	}
}
