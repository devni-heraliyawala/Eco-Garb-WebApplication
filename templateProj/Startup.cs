using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(templateProj.Startup))]
namespace templateProj
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
