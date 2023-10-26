using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(gridEditPopup.Startup))]
namespace gridEditPopup
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
