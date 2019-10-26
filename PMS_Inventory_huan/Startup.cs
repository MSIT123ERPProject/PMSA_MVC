using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PMS_Inventory_huan.Startup))]
namespace PMS_Inventory_huan
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
