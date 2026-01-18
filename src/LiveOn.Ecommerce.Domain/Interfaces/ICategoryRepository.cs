using LiveOn.Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveOn.Ecommerce.Domain.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> GetByNameAsync(string name);

        Task<Category> GetBySlugAsync(string slug);

        Task<IEnumerable<Category>> GetRootCategoriesAsync();

        Task<IEnumerable<Category>> GetParentCategoriesAsync(int parentId);

        Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentId);

        Task<IEnumerable<Category>> GetActiveCategoriesAsync();
    }
}
