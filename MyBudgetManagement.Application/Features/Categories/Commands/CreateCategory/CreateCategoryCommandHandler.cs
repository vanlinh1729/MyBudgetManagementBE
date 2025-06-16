using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Common.Interfaces;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities.Categories;

namespace MyBudgetManagement.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public CreateCategoryCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        // Không cho trùng tên cùng loại
        var isExist = await _uow.Categories.Query().AnyAsync(x =>
            x.UserId == userId &&
            x.Name.ToLower() == request.Name.Trim().ToLower() &&
            x.Type == request.Type);

        if (isExist)
            throw new ValidationException("Tên danh mục đã tồn tại trong loại này.");

        var category = new Category
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = request.Name.Trim(),
            Type = request.Type,
            Icon = request.Icon,
            Level = request.Level,
            Period = request.Period,
            IsDefault = request.IsDefault,
            Created = DateTime.UtcNow,
            CreatedBy = userId
        };

        await _uow.Categories.AddAsync(category);
        await _uow.SaveChangesAsync(cancellationToken);

        return category.Id;
    }
}
