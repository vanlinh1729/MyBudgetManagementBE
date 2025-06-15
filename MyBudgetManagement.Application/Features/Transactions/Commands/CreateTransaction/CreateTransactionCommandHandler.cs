using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Common.Interfaces;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Entities.Transactions;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Application.Features.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public CreateTransactionCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == request.CategoryId && c.UserId == userId, cancellationToken);

        if (category == null)
            throw new NotFoundException("Category not found for this user.");

        var transaction = new Transaction
        {
            CategoryId = request.CategoryId,
            Amount = request.Amount,
            Date = request.Date,
            Note = request.Note ?? string.Empty,
            Image = request.Image,
            CreatedBy = userId,
            Created = DateTime.UtcNow
        };

        _context.Transactions.Add(transaction);

        var userBalance = await _context.UserBalances
            .FirstOrDefaultAsync(b => b.UserId == userId, cancellationToken);

        if (userBalance != null)
        {
            userBalance.Balance += request.Amount * (category.Type == Domain.Enums.CategoryType.Income ? 1 : -1);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return transaction.Id;
    }
}