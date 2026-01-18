using LiveOn.Ecommerce.Domain.Interfaces;
using LiveOn.Ecommerce.Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveOn.Ecommerce.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private bool _disposed;

        private IProductRepository _products;
        private ICategoryRepository _categories;
        private IUserRepository _users;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IProductRepository Product
        {
            get
            {
                if (_products == null)
                    return new ProductRepository(_context);
                return _products;
            }
        }

        public ICategoryRepository Category
        {
            get
            {
                if (_categories == null)
                    return new CategoryRepository(_context);
                return _categories;
            }
        }

        public IUserRepository User
        {
            get
            {
                if (_users == null)
                    return new UserRepository(_context);
                return _users;
            }
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public Task<int> CompleteAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }
}
