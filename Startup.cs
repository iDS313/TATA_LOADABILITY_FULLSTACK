using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Loadability.Startup))]
namespace Loadability
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
