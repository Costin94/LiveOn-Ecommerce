using System.Web;
using System.Web.Http;

namespace LiveOn.Ecommerce.API
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
