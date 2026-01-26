using LiveOn.Ecommerce.Application.DTOs;
using LiveOn.Ecommerce.Application.Interfaces;
using LiveOn.Ecommerce.Application.Mappers;
using LiveOn.Ecommerce.Application.Queries.Categories;
using LiveOn.Ecommerce.Domain.Entities;
using LiveOn.Ecommerce.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveOn.Ecommerce.Application.Handlers.QueryHandlers.Categories
{
    public class GetAllCategoriesQueryHandler : IQueryHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllCategoriesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public IEnumerable<CategoryDto> Handle(GetAllCategoriesQuery query)
        {
            var categories = _unitOfWork.Category.GetAll();

            categories = ApplyFilters(categories, query);

            return categories.Select(c => c.MapToDto()).ToList();
		}

        public async Task<IEnumerable<CategoryDto>> HandleAsync(GetAllCategoriesQuery query)
        {
            var categories = await _unitOfWork.Category.GetAllAsync();

            categories = ApplyFilters(categories, query);

            return categories.Select(c => c.MapToDto()).ToList();
		}

        private IEnumerable<Category> ApplyFilters(
            IEnumerable<Category> categories,
            GetAllCategoriesQuery query)
        {
            if (query.IsActive.HasValue)
            {
                categories = categories.Where(c => c.IsActive == query.IsActive.Value);
            }
            
            return categories;
		}
	}
}
