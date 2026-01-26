using LiveOn.Ecommerce.Application.DTOs;
using LiveOn.Ecommerce.Domain.Entities;

namespace LiveOn.Ecommerce.Application.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryDto MapToDto(this Category category)
        {
            if (category == null)
                return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Slug = category.Slug,
                ParentCategoryId = category.ParentCategoryId,
                ParentCategoryName = category.ParentCategory?.Name,
                IsActive = category.IsActive,
                DisplayOrder = category.DisplayOrder,
                ProductCount = category.Products?.Count ?? 0,
                CreatedDate = category.CreatedAt,
                ModifiedDate = category.UpdatedAt
            };
        }
    }
}
