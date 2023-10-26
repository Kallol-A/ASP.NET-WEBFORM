using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MachineTest2.Startup))]
namespace MachineTest2
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
