using LiveOn.Ecommerce.Application.Interfaces;

namespace LiveOn.Ecommerce.Application.Commands.Categories
{
    /// <summary>
    /// Command to update an existing category
    /// </summary>
    public class UpdateCategoryCommand : ICommand<bool>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
    }
}
