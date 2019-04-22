using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Helpdesk.Website.Startup))]
namespace Helpdesk.Website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
