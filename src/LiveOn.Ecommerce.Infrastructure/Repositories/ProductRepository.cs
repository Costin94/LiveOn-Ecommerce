using LiveOn.Ecommerce.Domain.Entities;
using LiveOn.Ecommerce.Domain.Enums;
using LiveOn.Ecommerce.Domain.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace LiveOn.Ecommerce.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(DbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<Product> GetByNameAsync(string name)
        {
            return await _dbSet
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Name == name && !p.IsDeleted);
        }

        public async Task<Product> GetBySkuAsync(string sku)
        {
            return await _dbSet
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.SKU == sku && !p.IsDeleted);
        }

        public async Task<IEnumerable<Product>> GetByStatusAsync(ProductStatus status)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Where(p => p.Status == status && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetFeaturedProductsAsync()
        {
            return await _dbSet
                .Include(p => p.Category)
                .Where(p => p.IsFeatured && !p.IsDeleted).ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchAsync(string searchQuery)
        {
            var searchToLower = searchQuery.ToLower();
            return await _dbSet
                .Include(p => p.Category)
                .Where(p => (p.Name.ToLower().Contains(searchToLower) ||
                       p.Description.ToLower().Contains(searchToLower)) &&
                      !p.IsDeleted)
            .ToListAsync();
        }
    }
}
