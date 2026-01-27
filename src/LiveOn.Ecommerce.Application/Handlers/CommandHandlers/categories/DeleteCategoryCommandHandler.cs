using LiveOn.Ecommerce.Application.Commands.Categories;
using LiveOn.Ecommerce.Application.Interfaces;
using LiveOn.Ecommerce.Domain.Entities;
using LiveOn.Ecommerce.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveOn.Ecommerce.Application.Handlers.CommandHandlers.Categories
{
    public class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new NullReferenceException(nameof(unitOfWork));
        }

        public bool Handle(DeleteCategoryCommand command)
        {
            var category = _unitOfWork.Category.GetById(command.Id);

            if (category == null)
                return false;

            _unitOfWork.Category.Remove(category);
            _unitOfWork.Complete();

            return true;
        }

        public async Task<bool> HandleAsync(DeleteCategoryCommand command)
        {
            var category = _unitOfWork.Category.GetById(command.Id);

            if (category == null)
                return false;

            _unitOfWork.Category.Remove(category);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
