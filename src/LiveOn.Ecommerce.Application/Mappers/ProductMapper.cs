using LiveOn.Ecommerce.Application.DTOs;
using LiveOn.Ecommerce.Domain.Entities;

namespace LiveOn.Ecommerce.Application.Mappers
{
    public static class ProductMapper
    {
        public static ProductDto MapToDto(this Product product)
        {
            if (product == null)
                return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                SKU = product.SKU,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name,
                Status = product.Status,
                ImageUrl = product.ImageUrl,
                CreatedDate = product.CreatedAt,
                ModifiedDate = product.UpdatedAt
            };
        }
    }
}
