using MediatR;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Common.Interfaces;
using MyBudgetManagement.Application.Interfaces;

namespace MyBudgetManagement.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public UpdateCategoryCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _uow.Categories.GetByIdAsync(request.Id);
        if (category == null || category.UserId != _currentUser.UserId)
            throw new NotFoundException("Không tìm thấy danh mục.");

        category.Name = request.Name;
        category.Budget = request.Budget;
        category.Icon = request.Icon;
        category.Level = request.Level;
        category.Period = request.Period;

        _uow.Categories.Update(category);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}
