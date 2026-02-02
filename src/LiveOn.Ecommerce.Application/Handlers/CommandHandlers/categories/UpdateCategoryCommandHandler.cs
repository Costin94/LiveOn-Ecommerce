using LiveOn.Ecommerce.Application.Commands.Categories;
using LiveOn.Ecommerce.Application.Interfaces;
using LiveOn.Ecommerce.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveOn.Ecommerce.Application.Handlers.CommandHandlers.Categories
{
    public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public bool Handle(UpdateCategoryCommand command)
        {
            var category = _unitOfWork.Category.GetById(command.Id);

            if (category == null)
                return false;

            if(!string.IsNullOrEmpty(command.Name))
                category.SetName(command.Name);

            if (!string.IsNullOrEmpty(command.Description))
                category.SetDescription(command.Description);

            if (command.DisplayOrder != 0)
                category.SetDisplayOrder(command.DisplayOrder);

            _unitOfWork.Complete();

            return true;
        }

        public async Task<bool> HandleAsync(UpdateCategoryCommand command)
        {
            var category = await _unitOfWork.Category.GetByIdAsync(command.Id);

            if (category == null)
                return false;

            if (!string.IsNullOrEmpty(command.Name))
                category.SetName(command.Name);

            if (!string.IsNullOrEmpty(command.Description))
                category.SetDescription(command.Description);

            if (command.DisplayOrder != 0)
                category.SetDisplayOrder(command.DisplayOrder);

            await _unitOfWork.CompleteAsync();
            
            return true;
        }
    }
}
