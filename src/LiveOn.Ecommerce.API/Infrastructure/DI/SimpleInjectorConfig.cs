using System.Web.Http;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using LiveOn.Ecommerce.Application.Interfaces;
using LiveOn.Ecommerce.Application.Services;
using LiveOn.Ecommerce.Domain.Interfaces;
using LiveOn.Ecommerce.Infrastructure.Data.Context;
using LiveOn.Ecommerce.Infrastructure.Repositories;

namespace LiveOn.Ecommerce.API.Infrastructure.DI
{
    /// <summary>
    /// Simple Injector Dependency Injection Configuration
    /// Fastest DI container with excellent error messages and built-in verification
    /// Features: Performance, simplicity, compile-time verification
    /// </summary>
    public static class SimpleInjectorConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new Container();
            
            // Use AsyncScopedLifestyle for Web API
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            // === Core Infrastructure ===
            // Scoped = One instance per HTTP request
            container.Register<ApplicationDbContext>(Lifestyle.Scoped);
            container.Register<IUnitOfWork, UnitOfWork>(Lifestyle.Scoped);

            // === Application Services ===
            container.Register<IUserService, UserService>(Lifestyle.Scoped);

            // === Auto-Register All Command Handlers ===
            // This scans assemblies and registers all ICommandHandler<,> implementations
            var assemblies = new[] { typeof(ICommandHandler<,>).Assembly };
            container.Register(typeof(ICommandHandler<,>), assemblies, Lifestyle.Scoped);

            // === Auto-Register All Query Handlers ===
            // This scans assemblies and registers all IQueryHandler<,> implementations
            container.Register(typeof(IQueryHandler<,>), assemblies, Lifestyle.Scoped);

            // Register Web API controllers
            container.RegisterWebApiControllers(config);

            // Verify configuration (catches errors at startup!)
            container.Verify();

            // Set resolver
            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }
    }
}
