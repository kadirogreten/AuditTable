using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AuditTableWebApp.Startup))]
namespace AuditTableWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
