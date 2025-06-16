using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Common.Interfaces;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Application.Features.Transactions.Commands.DeleteTransaction;

public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public DeleteTransactionCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        var transaction = await _uow.Transactions
            .Query()
            .Include(t => t.Category)
            .Include(t => t.UserBalance)
            .FirstOrDefaultAsync(t => t.Id == request.Id && t.UserBalance.UserId == userId, cancellationToken);

        if (transaction == null)
            throw new NotFoundException("Không tìm thấy giao dịch.");

        var category = transaction.Category;
        var userBalance = transaction.UserBalance;

        // Rollback balance nếu không phải DebtAndLoan
        if (category.Type != CategoryType.DebtAndLoan)
        {
            if (category.Type == CategoryType.Income)
                userBalance.Balance -= transaction.Amount;
            else if (category.Type == CategoryType.Expense)
                userBalance.Balance += transaction.Amount;
        }

        // Rollback AmountPaid nếu là DebtAndLoan
        if (transaction.DebtAndLoanId.HasValue)
        {
            var debt = await _uow.DebtAndLoans.GetByIdAsync(transaction.DebtAndLoanId.Value);
            if (debt != null)
            {
                debt.AmountPaid -= transaction.Amount;

                if (debt.AmountPaid < debt.Amount)
                    debt.Status = PaymentStatus.Unpaid;
            }
        }

        _uow.Transactions.Remove(transaction);
        await _uow.SaveChangesAsync(cancellationToken);

    }
}