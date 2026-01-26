using LiveOn.Ecommerce.Application.DTOs;
using LiveOn.Ecommerce.Application.Interfaces;

namespace LiveOn.Ecommerce.Application.Queries.Products
{
    public class GetProductBySkuQuery : IQuery<ProductDto>
    {
        public string SKU { get; set; }

        public GetProductBySkuQuery(string sku)
        {
            SKU = sku;
        }
    }
}
