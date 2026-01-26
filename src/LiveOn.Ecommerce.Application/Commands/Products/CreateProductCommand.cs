using LiveOn.Ecommerce.Application.Interfaces;

namespace LiveOn.Ecommerce.Application.Commands.Products
{
    /// <summary>
    /// Command to create a new product
    /// </summary>
    public class CreateProductCommand : ICommand<int>
    {
        public string Name { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int InitialStock { get; set; }
        public string ImageUrl { get; set; }
    }
}
