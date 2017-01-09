using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(weatherwebinarService.Startup))]

namespace weatherwebinarService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}