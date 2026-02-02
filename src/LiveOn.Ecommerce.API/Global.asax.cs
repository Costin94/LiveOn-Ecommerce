using System.Web;
using System.Web.Http;
using LiveOn.Ecommerce.API.Infrastructure.DI;

namespace LiveOn.Ecommerce.API
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            
            // === Dependency Injection Configuration ===
            // Choose ONE of the following (currently using Autofac):
            
            // Option 1: Unity (Microsoft)
            // GlobalConfiguration.Configure(UnityConfig.Register);
            
            // Option 2: Autofac (CURRENT - Most Popular) ?
            GlobalConfiguration.Configure(AutofacConfig.Register);
            
            // Option 3: Simple Injector (Fastest Performance)
            // GlobalConfiguration.Configure(SimpleInjectorConfig.Register);
            
            // Option 4: Custom SimpleDependencyResolver
            // GlobalConfiguration.Configuration.DependencyResolver = new SimpleDependencyResolver();
        }
    }
}
