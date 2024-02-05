using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FP2Dev.Startup))]
namespace FP2Dev
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
