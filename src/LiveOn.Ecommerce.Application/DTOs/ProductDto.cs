using System;
using LiveOn.Ecommerce.Domain.Enums;

namespace LiveOn.Ecommerce.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for Product
    /// Used to transfer product data between layers
    /// </summary>
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public ProductStatus Status { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
