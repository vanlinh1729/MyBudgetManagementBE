using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Common.Interfaces;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Enums;

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

    public async Task Handle(DeleteCategoryCommand request, CancellationToken ct)
    {
        var userId = _currentUser.UserId;
        var category = await _uow.Categories.GetByIdAsync(request.Id);
        if (category == null || category.UserId != userId)
            throw new NotFoundException("Category không tồn tại.");

        var userBalance = await _uow.UserBalances.GetUserBalanceByUserIdAsync(userId);
        if (userBalance == null)
            throw new NotFoundException("Không tìm thấy số dư.");

        if (category.Type == CategoryType.DebtAndLoan)
        {
            var debtAndLoans = await _uow.DebtAndLoans
                .Query()
                .Where(d => d.CategoryId == request.Id && d.CreatedBy == userId)
                .Include(x=>x.Transactions)
                .ToListAsync();

            foreach (var debt in debtAndLoans)
            {
                foreach (var transaction in debt.Transactions)
                {
                    userBalance.Balance += transaction.Amount * (debt.IsDebt ? -1 : 1);
                    _uow.Transactions.Remove(transaction);
                }

                _uow.DebtAndLoans.Remove(debt);
            }
        }
        else
        {
            var transactions = await _uow.Transactions
                .Query()
                .Where(t => t.CategoryId == request.Id && t.CreatedBy == userId)
                .ToListAsync();

            foreach (var t in transactions)
            {
                userBalance.Balance += t.Amount * (category.Type == CategoryType.Expense ? 1 : -1);
                _uow.Transactions.Remove(t);
            }
        }

        _uow.Categories.Remove(category);
        await _uow.SaveChangesAsync(ct);
    }

}
