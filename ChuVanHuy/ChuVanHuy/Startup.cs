using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ChuVanHuy.Startup))]
namespace ChuVanHuy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
