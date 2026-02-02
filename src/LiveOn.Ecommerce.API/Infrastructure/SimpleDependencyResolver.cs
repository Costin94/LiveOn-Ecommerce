using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using LiveOn.Ecommerce.Application.Commands.Products;
using LiveOn.Ecommerce.Application.DTOs;
using LiveOn.Ecommerce.Application.Handlers.CommandHandlers.Products;
using LiveOn.Ecommerce.Application.Handlers.QueryHandlers.Products;
using LiveOn.Ecommerce.Application.Interfaces;
using LiveOn.Ecommerce.Domain.Interfaces;
using LiveOn.Ecommerce.Infrastructure.Data.Context;
using LiveOn.Ecommerce.Infrastructure.Repositories;
using LiveOn.Ecommerce.Application.Commands.Categories;
using LiveOn.Ecommerce.Application.Handlers.CommandHandlers.Categories;
using LiveOn.Ecommerce.Application.Handlers.QueryHandlers.Categories;
using LiveOn.Ecommerce.Application.Queries.Categories;
using LiveOn.Ecommerce.Application.Services;
using LiveOn.Ecommerce.Application.Queries.Products;

namespace LiveOn.Ecommerce.API.Infrastructure
{
    /// <summary>
    /// Simple dependency resolver for Web API
    /// Implements per-request scope for DbContext and proper disposal
    /// </summary>
    public class SimpleDependencyResolver : IDependencyResolver
    {
        private readonly Dictionary<Type, Func<object>> _services = new Dictionary<Type, Func<object>>();
        
        // Per-request instances (disposed at end of request)
        private ApplicationDbContext _dbContext;
        private IUnitOfWork _unitOfWork;
        private bool _disposed;

        public SimpleDependencyResolver()
        {
            RegisterServices();
        }

        private void RegisterServices()
        {
            // DbContext - ONE per request (lazy created)
            _services[typeof(ApplicationDbContext)] = () => 
            {
                if (_dbContext == null)
                    _dbContext = new ApplicationDbContext();
                return _dbContext;
            };

            // Unit of Work - ONE per request, shares DbContext
            _services[typeof(IUnitOfWork)] = () => 
            {
                if (_unitOfWork == null)
                    _unitOfWork = new UnitOfWork(GetService<ApplicationDbContext>());
                return _unitOfWork;
            };

            // User Service
            _services[typeof(IUserService)] = () => 
                new UserService(GetService<IUnitOfWork>());

            // Command Handlers - Products
            _services[typeof(ICommandHandler<CreateProductCommand, int>)] = () => 
                new CreateProductCommandHandler(GetService<IUnitOfWork>());
            
            _services[typeof(ICommandHandler<UpdateProductCommand, bool>)] = () => 
                new UpdateProductCommandHandler(GetService<IUnitOfWork>());
            
            _services[typeof(ICommandHandler<DeleteProductCommand, bool>)] = () => 
                new DeleteProductCommandHandler(GetService<IUnitOfWork>());
            
            _services[typeof(ICommandHandler<UpdateProductStockCommand, bool>)] = () => 
                new UpdateProductStockCommandHandler(GetService<IUnitOfWork>());

            // Query Handlers - Products
            _services[typeof(IQueryHandler<GetProductByIdQuery, ProductDto>)] = () => 
                new GetProductByIdQueryHandler(GetService<IUnitOfWork>());

            _services[typeof(IQueryHandler<GetAllProductsQuery, IEnumerable<ProductDto>>)] = () => 
                new GetAllProductsQueryHandler(GetService<IUnitOfWork>());
            
            _services[typeof(IQueryHandler<GetProductBySkuQuery, ProductDto>)] = () => 
                new GetProductBySkuQueryHandler(GetService<IUnitOfWork>());

            // Command Handlers - Categories
            _services[typeof(ICommandHandler<CreateCategoryCommand, int>)] = () => 
                new CreateCategoryCommandHandler(GetService<IUnitOfWork>());
            
            _services[typeof(ICommandHandler<UpdateCategoryCommand, bool>)] = () => 
                new UpdateCategoryCommandHandler(GetService<IUnitOfWork>());
            
            _services[typeof(ICommandHandler<DeleteCategoryCommand, bool>)] = () => 
                new DeleteCategoryCommandHandler(GetService<IUnitOfWork>());

            // Query Handlers - Categories
            _services[typeof(IQueryHandler<GetCategoryByIdQuery, CategoryDto>)] = () => 
                new GetCategoryByIdQueryHandler(GetService<IUnitOfWork>());
            
            _services[typeof(IQueryHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>)] = () => 
                new GetAllCategoriesQueryHandler(GetService<IUnitOfWork>());
        }

        public object GetService(Type serviceType)
        {
            if (_services.ContainsKey(serviceType))
            {
                return _services[serviceType]();  // Execute factory
            }
            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (_services.ContainsKey(serviceType))
            {
                yield return _services[serviceType]();
            }
        }

        public IDependencyScope BeginScope()
        {
            // Create new scope for each request
            return new SimpleDependencyResolver();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _unitOfWork?.Dispose();
                _dbContext?.Dispose();
                _disposed = true;
            }
        }

        private T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }
    }
}
