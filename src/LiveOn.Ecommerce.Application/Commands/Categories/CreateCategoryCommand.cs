using LiveOn.Ecommerce.Application.Interfaces;

namespace LiveOn.Ecommerce.Application.Commands.Categories
{
    /// <summary>
    /// Command to create a new category
    /// </summary>
    public class CreateCategoryCommand : ICommand<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentCategoryId { get; set; }
        public int DisplayOrder { get; set; }
    }
}
