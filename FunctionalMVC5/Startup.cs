using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FunctionalMVC5.Startup))]
namespace FunctionalMVC5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
