using System.Collections.Generic;
using LiveOn.Ecommerce.Application.DTOs;
using LiveOn.Ecommerce.Application.Interfaces;

namespace LiveOn.Ecommerce.Application.Queries.Categories
{
    public class GetAllCategoriesQuery : IQuery<IEnumerable<CategoryDto>>
    {
        public bool? IsActive { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
