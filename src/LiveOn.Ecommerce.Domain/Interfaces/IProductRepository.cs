using LiveOn.Ecommerce.Domain.Entities;
using LiveOn.Ecommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveOn.Ecommerce.Domain.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetBySkuAsync(string sku);

        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);

        Task<IEnumerable<Product>> GetFeaturedProductsAsync();

        Task<Product> GetByNameAsync(string name);

        Task<IEnumerable<Product>> SearchAsync(string searchQuery);

        Task<IEnumerable<Product>> GetByStatusAsync(ProductStatus status);
    }
}
