using System;
using System.Threading.Tasks;
using LiveOn.Ecommerce.Application.Commands.Products;
using LiveOn.Ecommerce.Application.Interfaces;
using LiveOn.Ecommerce.Domain.Entities;
using LiveOn.Ecommerce.Domain.Interfaces;

namespace LiveOn.Ecommerce.Application.Handlers.CommandHandlers.Products
{
    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public int Handle(CreateProductCommand command)
        {
            var product = new Product(
                name: command.Name,
                sku: command.SKU,
                price: command.Price,
                categoryId: command.CategoryId,
                description: command.Description
            );

            if (command.InitialStock > 0)
            {
                product.IncreaseStock(command.InitialStock);
            }

            if (!string.IsNullOrEmpty(command.ImageUrl))
            {
                product.SetImageUrl(command.ImageUrl);
            }

            _unitOfWork.Product.Add(product);
            _unitOfWork.Complete();

            return product.Id;
        }

        public async Task<int> HandleAsync(CreateProductCommand command)
        {
            var product = new Product(
                name: command.Name,
                sku: command.SKU,
                price: command.Price,
                categoryId: command.CategoryId,
                description: command.Description
            );

            if (command.InitialStock > 0)
            {
                product.IncreaseStock(command.InitialStock);
            }

            if (!string.IsNullOrEmpty(command.ImageUrl))
            {
                product.SetImageUrl(command.ImageUrl);
            }

            _unitOfWork.Product.Add(product);
            await _unitOfWork.CompleteAsync();

            return product.Id;
        }
    }
}
