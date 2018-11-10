using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCATM.Startup))]
namespace MVCATM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
