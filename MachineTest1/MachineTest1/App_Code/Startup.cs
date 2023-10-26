using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MachineTest1.Startup))]
namespace MachineTest1
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
