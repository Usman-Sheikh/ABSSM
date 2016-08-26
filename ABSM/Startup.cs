using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ABSM.Startup))]
namespace ABSM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
