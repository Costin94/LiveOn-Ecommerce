using LiveOn.Ecommerce.Application.Interfaces;

namespace LiveOn.Ecommerce.Application.Commands.Products
{
    /// <summary>
    /// Command to delete a product
    /// </summary>
    public class DeleteProductCommand : ICommand<bool>
    {
        public int Id { get; set; }
    }
}
