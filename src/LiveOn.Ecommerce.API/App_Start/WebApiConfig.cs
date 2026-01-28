using System.Web.Http;
using LiveOn.Ecommerce.API.Infrastructure;

namespace LiveOn.Ecommerce.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configure Dependency Injection
            config.DependencyResolver = new SimpleDependencyResolver();

            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
