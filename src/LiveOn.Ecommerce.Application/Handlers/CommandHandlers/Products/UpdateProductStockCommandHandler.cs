using System;
using System.Threading.Tasks;
using LiveOn.Ecommerce.Application.Commands.Products;
using LiveOn.Ecommerce.Application.Interfaces;
using LiveOn.Ecommerce.Domain.Interfaces;

namespace LiveOn.Ecommerce.Application.Handlers.CommandHandlers.Products
{
    /// <summary>
    /// Handler for UpdateProductStockCommand
    /// Updates the stock quantity for a product
    /// </summary>
    public class UpdateProductStockCommandHandler : ICommandHandler<UpdateProductStockCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductStockCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public bool Handle(UpdateProductStockCommand command)
        {
            var product = _unitOfWork.Product.GetById(command.ProductId);
            if (product == null)
                return false;

            if (command.Quantity > 0)
            {
                product.IncreaseStock(command.Quantity);
            }
            else if (command.Quantity < 0)
            {
                product.DecreaseStock(Math.Abs(command.Quantity));
            }

            _unitOfWork.Product.Update(product);
            _unitOfWork.Complete();

            return true;
        }

        public async Task<bool> HandleAsync(UpdateProductStockCommand command)
        {
            var product = await _unitOfWork.Product.GetByIdAsync(command.ProductId);
            if (product == null)
                return false;

            if (command.Quantity > 0)
            {
                product.IncreaseStock(command.Quantity);
            }
            else if (command.Quantity < 0)
            {
                product.DecreaseStock(Math.Abs(command.Quantity));
            }

            _unitOfWork.Product.Update(product);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
