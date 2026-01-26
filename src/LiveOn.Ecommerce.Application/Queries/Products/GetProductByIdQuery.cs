using LiveOn.Ecommerce.Application.DTOs;
using LiveOn.Ecommerce.Application.Interfaces;

namespace LiveOn.Ecommerce.Application.Queries.Products
{
    public class GetProductByIdQuery : IQuery<ProductDto>
    {
        public int Id { get; set; }

        public GetProductByIdQuery(int id)
        {
            Id = id;
        }
    }
}
