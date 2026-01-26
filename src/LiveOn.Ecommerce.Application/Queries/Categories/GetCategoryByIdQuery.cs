using LiveOn.Ecommerce.Application.DTOs;
using LiveOn.Ecommerce.Application.Interfaces;

namespace LiveOn.Ecommerce.Application.Queries.Categories
{
    public class GetCategoryByIdQuery : IQuery<CategoryDto>
    {
        public int Id { get; set; }

        public GetCategoryByIdQuery(int id)
        {
            Id = id;
        }
    }
}
