using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Online_Bookstore.Startup))]
namespace Online_Bookstore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
