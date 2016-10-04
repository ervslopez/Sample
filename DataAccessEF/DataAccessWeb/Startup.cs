using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DataAccessWeb.Startup))]
namespace DataAccessWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
