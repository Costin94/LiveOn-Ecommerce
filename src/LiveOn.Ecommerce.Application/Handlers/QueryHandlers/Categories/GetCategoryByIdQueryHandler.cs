using LiveOn.Ecommerce.Application.DTOs;
using LiveOn.Ecommerce.Application.Interfaces;
using LiveOn.Ecommerce.Application.Mappers;
using LiveOn.Ecommerce.Application.Queries.Categories;
using LiveOn.Ecommerce.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace LiveOn.Ecommerce.Application.Handlers.QueryHandlers.Categories
{
    public class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, CategoryDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public CategoryDto Handle(GetCategoryByIdQuery query)
        {
            var category = _unitOfWork.Category.GetById(query.Id);
            return category?.MapToDto();
        }

        public async Task<CategoryDto> HandleAsync(GetCategoryByIdQuery query)
        {
            var category = await _unitOfWork.Category.GetByIdAsync(query.Id);
            return category?.MapToDto();
        }
    }
}

