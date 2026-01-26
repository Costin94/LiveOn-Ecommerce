using System.Collections.Generic;
using LiveOn.Ecommerce.Application.DTOs;
using LiveOn.Ecommerce.Application.Interfaces;

namespace LiveOn.Ecommerce.Application.Queries.Products
{
    public class GetAllProductsQuery : IQuery<IEnumerable<ProductDto>>
    {
        public int? CategoryId { get; set; }
        public string SearchTerm { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? InStock { get; set; }
    }
}
