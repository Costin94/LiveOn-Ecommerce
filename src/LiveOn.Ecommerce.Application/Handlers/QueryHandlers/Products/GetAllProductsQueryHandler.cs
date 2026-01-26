using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveOn.Ecommerce.Application.DTOs;
using LiveOn.Ecommerce.Application.Interfaces;
using LiveOn.Ecommerce.Application.Mappers;
using LiveOn.Ecommerce.Application.Queries.Products;
using LiveOn.Ecommerce.Domain.Entities;
using LiveOn.Ecommerce.Domain.Interfaces;

namespace LiveOn.Ecommerce.Application.Handlers.QueryHandlers.Products
{
    public class GetAllProductsQueryHandler : IQueryHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllProductsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public IEnumerable<ProductDto> Handle(GetAllProductsQuery query)
        {
            var products = _unitOfWork.Product.GetAll();

            products = ApplyFilters(products, query);

            return products.Select(p => p.MapToDto()).ToList();
        }

        public async Task<IEnumerable<ProductDto>> HandleAsync(GetAllProductsQuery query)
        {
            var products = await _unitOfWork.Product.GetAllAsync();

            products = ApplyFilters(products, query);

            return products.Select(p => p.MapToDto()).ToList();
        }

        private IEnumerable<Product> ApplyFilters(
            IEnumerable<Product> products, 
            GetAllProductsQuery query)
        {
            if (query.CategoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == query.CategoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                var searchLower = query.SearchTerm.ToLower();
                products = products.Where(p => 
                    p.Name.ToLower().Contains(searchLower) || 
                    (p.Description != null && p.Description.ToLower().Contains(searchLower)) ||
                    p.SKU.ToLower().Contains(searchLower));
            }

            if (query.MinPrice.HasValue)
            {
                products = products.Where(p => p.Price >= query.MinPrice.Value);
            }

            if (query.MaxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= query.MaxPrice.Value);
            }

            if (query.InStock.HasValue && query.InStock.Value)
            {
                products = products.Where(p => p.StockQuantity > 0);
            }

            return products;
        }

        private ProductDto MapToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                SKU = product.SKU,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name,
                Status = product.Status,
                ImageUrl = product.ImageUrl,
                CreatedDate = product.CreatedAt,
                ModifiedDate = product.UpdatedAt
            };
        }
    }
}
