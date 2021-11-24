using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Gyak9_ZEACDR.StartupOwin))]

namespace Gyak9_ZEACDR
{
    public partial class StartupOwin
    {
        public void Configuration(IAppBuilder app)
        {
            //AuthStartup.ConfigureAuth(app);
        }
    }
}
