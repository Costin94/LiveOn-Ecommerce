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
    public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProductByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public ProductDto Handle(GetProductByIdQuery query)
        {
            var product = _unitOfWork.Product.GetById(query.Id);
            return product?.MapToDto();
        }

        public async Task<ProductDto> HandleAsync(GetProductByIdQuery query)
        {
            var product = await _unitOfWork.Product.GetByIdAsync(query.Id);
            return product?.MapToDto();
        }
    }
}
