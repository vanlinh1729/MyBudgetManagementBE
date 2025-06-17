using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Common.Interfaces;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities.Debts;

namespace MyBudgetManagement.Application.Features.DebtAndLoans.Commands.DeleteDebtAndLoan;

public class DeleteDebtAndLoanCommandHandler : IRequestHandler<DeleteDebtAndLoanCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public DeleteDebtAndLoanCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task Handle(DeleteDebtAndLoanCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        // Load DebtAndLoan bao gồm Transactions và Category
        var debt = await _uow.DebtAndLoans
            .Query()
            .Include(d => d.Transactions)
            .FirstOrDefaultAsync(d => d.Id == request.Id && d.CreatedBy == userId, cancellationToken);

        if (debt == null)
            throw new NotFoundException("Không tìm thấy khoản nợ/cho vay.");

        var userBalance = await _uow.UserBalances.GetUserBalanceByUserIdAsync(userId);
        if (userBalance == null)
            throw new NotFoundException("Không tìm thấy số dư.");

        // Tổng tiền đã giao dịch
        var total = debt.Transactions.Sum(t => t.Amount);

        // Đảo ngược thay đổi số dư
        if (debt.IsDebt)
        {
            // Mình vay → trước đó cộng tiền → giờ xoá → trừ lại
            userBalance.Balance -= total;
        }
        else
        {
            // Mình cho vay → trước đó trừ tiền → giờ xoá → cộng lại
            userBalance.Balance += total;
        }

        // Xoá các transaction liên quan
        _uow.Transactions.RemoveRange(debt.Transactions);

        // Xoá chính nó
        _uow.DebtAndLoans.Remove(debt);

        await _uow.SaveChangesAsync(cancellationToken);

    }
}
