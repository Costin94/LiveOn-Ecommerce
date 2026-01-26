using System;
using System.Threading.Tasks;
using LiveOn.Ecommerce.Application.Commands.Products;
using LiveOn.Ecommerce.Application.Interfaces;
using LiveOn.Ecommerce.Domain.Interfaces;

namespace LiveOn.Ecommerce.Application.Handlers.CommandHandlers.Products
{
    /// <summary>
    /// Handler for DeleteProductCommand
    /// Deletes a product from the system
    /// </summary>
    public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public bool Handle(DeleteProductCommand command)
        {
            var product = _unitOfWork.Product.GetById(command.Id);
            if (product == null)
                return false;

            _unitOfWork.Product.Remove(product);
            _unitOfWork.Complete();

            return true;
        }

        public async Task<bool> HandleAsync(DeleteProductCommand command)
        {
            var product = await _unitOfWork.Product.GetByIdAsync(command.Id);
            if (product == null)
                return false;

            _unitOfWork.Product.Remove(product);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
