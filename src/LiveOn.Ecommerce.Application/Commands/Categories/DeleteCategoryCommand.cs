using LiveOn.Ecommerce.Application.Interfaces;

namespace LiveOn.Ecommerce.Application.Commands.Categories
{
    /// <summary>
    /// Command to delete a category
    /// </summary>
    public class DeleteCategoryCommand : ICommand<bool>
    {
        public int Id { get; set; }
    }
}
