using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjectEstimation.Startup))]
namespace ProjectEstimation
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
