using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using LiveOn.Ecommerce.Application.Interfaces;
using LiveOn.Ecommerce.Application.Services;
using LiveOn.Ecommerce.Domain.Interfaces;
using LiveOn.Ecommerce.Infrastructure.Data.Context;
using LiveOn.Ecommerce.Infrastructure.Repositories;

namespace LiveOn.Ecommerce.API.Infrastructure.DI
{
    /// <summary>
    /// Autofac Dependency Injection Configuration
    /// Most popular DI container in .NET Framework ecosystem
    /// Features: Excellent lifetime management, auto-registration, powerful scoping
    /// </summary>
    public static class AutofacConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            // === Core Infrastructure ===
            // InstancePerRequest = One instance per HTTP request (disposed automatically)
            builder.RegisterType<ApplicationDbContext>()
                .AsSelf()
                .InstancePerRequest();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerRequest();

            // === Application Services ===
            builder.RegisterType<UserService>()
                .As<IUserService>()
                .InstancePerRequest();

            // === Auto-Register All Command Handlers ===
            // This scans the Application assembly and registers all ICommandHandler implementations
            builder.RegisterAssemblyTypes(typeof(ICommandHandler<,>).Assembly)
                .AsClosedTypesOf(typeof(ICommandHandler<,>))
                .InstancePerRequest();

            // === Auto-Register All Query Handlers ===
            // This scans the Application assembly and registers all IQueryHandler implementations
            builder.RegisterAssemblyTypes(typeof(IQueryHandler<,>).Assembly)
                .AsClosedTypesOf(typeof(IQueryHandler<,>))
                .InstancePerRequest();

            // === Register API Controllers ===
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Build container
            var container = builder.Build();

            // Set resolver
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
