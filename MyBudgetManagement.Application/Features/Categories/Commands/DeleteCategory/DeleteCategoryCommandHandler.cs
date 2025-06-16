using MediatR;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Common.Interfaces;
using MyBudgetManagement.Application.Interfaces;

namespace MyBudgetManagement.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public DeleteCategoryCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _uow.Categories.GetByIdAsync(request.Id);
        if (category == null || category.UserId != _currentUser.UserId)
            throw new NotFoundException("Không tìm thấy danh mục.");

        // TODO: Kiểm tra có transaction liên quan không → quyết định xóa cứng hay mềm
        _uow.Categories.Remove(category);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}
