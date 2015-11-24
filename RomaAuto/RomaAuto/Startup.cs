using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RomaAuto.Startup))]
namespace RomaAuto
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
