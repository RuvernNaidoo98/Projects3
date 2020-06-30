using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sprint33.Startup))]
namespace Sprint33
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
