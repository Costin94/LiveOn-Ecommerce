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
    public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new NullReferenceException(nameof(unitOfWork));
        }

        public int Handle(CreateCategoryCommand command)
        {
            var category = new Category
            (
                name: command.Name,
                description: command.Description
            );

            _unitOfWork.Category.Add(category);
            _unitOfWork.Complete();

            return category.Id;
        }

        public async Task<int> HandleAsync(CreateCategoryCommand command)
        {
            var category = new Category
            (
                name: command.Name,
                description: command.Description
            );

            _unitOfWork.Category.Add(category);
            await _unitOfWork.CompleteAsync();

            return category.Id;
        }
    }
}
