using System;
using System.Linq;
using System.Threading.Tasks;
using LiveOn.Ecommerce.Application.DTOs;
using LiveOn.Ecommerce.Application.Interfaces;
using LiveOn.Ecommerce.Application.Mappers;
using LiveOn.Ecommerce.Application.Queries.Products;
using LiveOn.Ecommerce.Domain.Interfaces;

namespace LiveOn.Ecommerce.Application.Handlers.QueryHandlers.Products
{
    public class GetProductBySkuQueryHandler : IQueryHandler<GetProductBySkuQuery, ProductDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProductBySkuQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public ProductDto Handle(GetProductBySkuQuery query)
        {
            var product = _unitOfWork.Product.FirstOrDefault(p => p.SKU == query.SKU);
            return product?.MapToDto();
        }

        public async Task<ProductDto> HandleAsync(GetProductBySkuQuery query)
        {
            var product = await _unitOfWork.Product.FirstOrDefaultAsync(p => p.SKU == query.SKU);
            return product?.MapToDto();
        }
    }
}
