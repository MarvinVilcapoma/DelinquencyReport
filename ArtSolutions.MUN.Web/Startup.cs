using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ArtSolutions.MUN.Web.Startup))]
namespace ArtSolutions.MUN.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}