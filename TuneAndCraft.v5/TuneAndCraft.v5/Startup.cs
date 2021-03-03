using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TuneAndCraft.v5.Startup))]
namespace TuneAndCraft.v5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
