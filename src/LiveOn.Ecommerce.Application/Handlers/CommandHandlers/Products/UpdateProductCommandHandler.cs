using System;
using System.Threading.Tasks;
using LiveOn.Ecommerce.Application.Commands.Products;
using LiveOn.Ecommerce.Application.Interfaces;
using LiveOn.Ecommerce.Domain.Interfaces;

namespace LiveOn.Ecommerce.Application.Handlers.CommandHandlers.Products
{
    /// <summary>
    /// Handler for UpdateProductCommand
    /// Updates an existing product
    /// </summary>
    public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public bool Handle(UpdateProductCommand command)
        {
            var product = _unitOfWork.Product.GetById(command.Id);
            if (product == null)
                return false;

            product.SetName(command.Name);
            product.SetDescription(command.Description);
            product.SetPrice(command.Price);
            product.ChangeCategory(command.CategoryId);

            if (!string.IsNullOrEmpty(command.ImageUrl))
            {
                product.SetImageUrl(command.ImageUrl);
            }

            _unitOfWork.Product.Update(product);
            _unitOfWork.Complete();

            return true;
        }

        public async Task<bool> HandleAsync(UpdateProductCommand command)
        {
            var product = await _unitOfWork.Product.GetByIdAsync(command.Id);
            if (product == null)
                return false;

            product.SetName(command.Name);
            product.SetDescription(command.Description);
            product.SetPrice(command.Price);
            product.ChangeCategory(command.CategoryId);

            if (!string.IsNullOrEmpty(command.ImageUrl))
            {
                product.SetImageUrl(command.ImageUrl);
            }

            _unitOfWork.Product.Update(product);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
