using LiveOn.Ecommerce.Application.Interfaces;

namespace LiveOn.Ecommerce.Application.Commands.Products
{
    /// <summary>
    /// Command to update product stock quantity
    /// </summary>
    public class UpdateProductStockCommand : ICommand<bool>
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
