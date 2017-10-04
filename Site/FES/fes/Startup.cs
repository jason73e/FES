using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(fes.Startup))]
namespace fes
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
