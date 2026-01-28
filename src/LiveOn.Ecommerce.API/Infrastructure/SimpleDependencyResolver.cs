using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using LiveOn.Ecommerce.Application.Commands.Products;
using LiveOn.Ecommerce.Application.DTOs;
using LiveOn.Ecommerce.Application.Handlers.CommandHandlers.Products;
using LiveOn.Ecommerce.Application.Handlers.QueryHandlers.Products;
using LiveOn.Ecommerce.Application.Interfaces;
using LiveOn.Ecommerce.Application.Queries.Products;
using LiveOn.Ecommerce.Domain.Interfaces;
using LiveOn.Ecommerce.Infrastructure.Data.Context;
using LiveOn.Ecommerce.Infrastructure.Repositories;
using LiveOn.Ecommerce.Application.Commands.Categories;
using LiveOn.Ecommerce.Application.Handlers.CommandHandlers.Categories;
using LiveOn.Ecommerce.Application.Handlers.QueryHandlers.Categories;
using LiveOn.Ecommerce.Application.Queries.Categories;

namespace LiveOn.Ecommerce.API.Infrastructure
{
    /// <summary>
    /// Simple dependency resolver for Web API
    /// Implements service locator pattern for dependency injection
    /// </summary>
    public class SimpleDependencyResolver : IDependencyResolver
    {
        private readonly Dictionary<Type, Func<object>> _services = new Dictionary<Type, Func<object>>();

        public SimpleDependencyResolver()
        {
            RegisterServices();
        }

        private void RegisterServices()
        {
            // Register DbContext (per request scope)
            _services[typeof(ApplicationDbContext)] = () => new ApplicationDbContext();

            // Register Unit of Work (per request)
            _services[typeof(IUnitOfWork)] = () => new UnitOfWork(new ApplicationDbContext());

            // Register Command Handlers - Products
            _services[typeof(ICommandHandler<CreateProductCommand, int>)] = () => 
                new CreateProductCommandHandler(GetService<IUnitOfWork>());
            
            _services[typeof(ICommandHandler<UpdateProductCommand, bool>)] = () => 
                new UpdateProductCommandHandler(GetService<IUnitOfWork>());
            
            _services[typeof(ICommandHandler<DeleteProductCommand, bool>)] = () => 
                new DeleteProductCommandHandler(GetService<IUnitOfWork>());
            
            _services[typeof(ICommandHandler<UpdateProductStockCommand, bool>)] = () => 
                new UpdateProductStockCommandHandler(GetService<IUnitOfWork>());

            // Register Query Handlers - Products
            _services[typeof(IQueryHandler<GetProductByIdQuery, ProductDto>)] = () => 
                new GetProductByIdQueryHandler(GetService<IUnitOfWork>());
            
            _services[typeof(IQueryHandler<GetAllProductsQuery, IEnumerable<ProductDto>>)] = () => 
                new GetAllProductsQueryHandler(GetService<IUnitOfWork>());
            
            _services[typeof(IQueryHandler<GetProductBySkuQuery, ProductDto>)] = () => 
                new GetProductBySkuQueryHandler(GetService<IUnitOfWork>());

            // Register Command Handlers - Categories
            _services[typeof(ICommandHandler<CreateCategoryCommand, int>)] = () => 
                new CreateCategoryCommandHandler(GetService<IUnitOfWork>());
            
            _services[typeof(ICommandHandler<UpdateCategoryCommand, bool>)] = () => 
                new UpdateCategoryCommandHandler(GetService<IUnitOfWork>());
            
            _services[typeof(ICommandHandler<DeleteCategoryCommand, bool>)] = () => 
                new DeleteCategoryCommandHandler(GetService<IUnitOfWork>());

            // Register Query Handlers - Categories
            _services[typeof(IQueryHandler<GetCategoryByIdQuery, CategoryDto>)] = () => 
                new GetCategoryByIdQueryHandler(GetService<IUnitOfWork>());
            
            _services[typeof(IQueryHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>)] = () => 
                new GetAllCategoriesQueryHandler(GetService<IUnitOfWork>());
        }

        public object GetService(Type serviceType)
        {
            if (_services.ContainsKey(serviceType))
            {
                return _services[serviceType];
            }
            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (_services.ContainsKey(serviceType))
            {
                yield return _services[serviceType];
            }
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public void Dispose()
        {
            // Cleanup if needed
        }

        private T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }
    }
}
