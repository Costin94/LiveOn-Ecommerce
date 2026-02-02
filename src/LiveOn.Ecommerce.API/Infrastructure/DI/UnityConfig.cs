using System.Web.Http;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;
using LiveOn.Ecommerce.Application.Commands.Categories;
using LiveOn.Ecommerce.Application.Commands.Products;
using LiveOn.Ecommerce.Application.DTOs;
using LiveOn.Ecommerce.Application.Handlers.CommandHandlers.Categories;
using LiveOn.Ecommerce.Application.Handlers.CommandHandlers.Products;
using LiveOn.Ecommerce.Application.Handlers.QueryHandlers.Categories;
using LiveOn.Ecommerce.Application.Handlers.QueryHandlers.Products;
using LiveOn.Ecommerce.Application.Interfaces;
using LiveOn.Ecommerce.Application.Queries.Categories;
using LiveOn.Ecommerce.Application.Queries.Products;
using LiveOn.Ecommerce.Application.Services;
using LiveOn.Ecommerce.Domain.Interfaces;
using LiveOn.Ecommerce.Infrastructure.Data.Context;
using LiveOn.Ecommerce.Infrastructure.Repositories;
using System.Collections.Generic;

namespace LiveOn.Ecommerce.API.Infrastructure.DI
{
    /// <summary>
    /// Unity Dependency Injection Configuration
    /// Microsoft's official DI container for .NET Framework
    /// </summary>
    public static class UnityConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();

            // === Core Infrastructure ===
            // HierarchicalLifetimeManager = One instance per request in Web API
            container.RegisterType<ApplicationDbContext>(new HierarchicalLifetimeManager());
            container.RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager());

            // === Application Services ===
            container.RegisterType<IUserService, UserService>(new HierarchicalLifetimeManager());

            // === Product Command Handlers ===
            container.RegisterType<ICommandHandler<CreateProductCommand, int>, CreateProductCommandHandler>(
                new HierarchicalLifetimeManager());
            
            container.RegisterType<ICommandHandler<UpdateProductCommand, bool>, UpdateProductCommandHandler>(
                new HierarchicalLifetimeManager());
            
            container.RegisterType<ICommandHandler<DeleteProductCommand, bool>, DeleteProductCommandHandler>(
                new HierarchicalLifetimeManager());
            
            container.RegisterType<ICommandHandler<UpdateProductStockCommand, bool>, UpdateProductStockCommandHandler>(
                new HierarchicalLifetimeManager());

            // === Product Query Handlers ===
            container.RegisterType<IQueryHandler<GetProductByIdQuery, ProductDto>, GetProductByIdQueryHandler>(
                new HierarchicalLifetimeManager());
            
            container.RegisterType<IQueryHandler<GetAllProductsQuery, IEnumerable<ProductDto>>, GetAllProductsQueryHandler>(
                new HierarchicalLifetimeManager());
            
            container.RegisterType<IQueryHandler<GetProductBySkuQuery, ProductDto>, GetProductBySkuQueryHandler>(
                new HierarchicalLifetimeManager());

            // === Category Command Handlers ===
            container.RegisterType<ICommandHandler<CreateCategoryCommand, int>, CreateCategoryCommandHandler>(
                new HierarchicalLifetimeManager());
            
            container.RegisterType<ICommandHandler<UpdateCategoryCommand, bool>, UpdateCategoryCommandHandler>(
                new HierarchicalLifetimeManager());
            
            container.RegisterType<ICommandHandler<DeleteCategoryCommand, bool>, DeleteCategoryCommandHandler>(
                new HierarchicalLifetimeManager());

            // === Category Query Handlers ===
            container.RegisterType<IQueryHandler<GetCategoryByIdQuery, CategoryDto>, GetCategoryByIdQueryHandler>(
                new HierarchicalLifetimeManager());
            
            container.RegisterType<IQueryHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>, GetAllCategoriesQueryHandler>(
                new HierarchicalLifetimeManager());

            // Set resolver for Web API
            config.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}
