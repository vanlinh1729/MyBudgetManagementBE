using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Common.Interfaces;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities.Transactions;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Application.Features.DebtAndLoans.Commands.PayDebtAndLoan;

public class PayDebtAndLoanCommandHandler : IRequestHandler<PayDebtAndLoanCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public PayDebtAndLoanCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task Handle(PayDebtAndLoanCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        var debt = await _uow.DebtAndLoans.Query()
            .Include(d => d.Category)
            .FirstOrDefaultAsync(d => d.Id == request.DebtAndLoanId && d.CreatedBy == userId, cancellationToken);

        if (debt == null)
            throw new NotFoundException("Không tìm thấy khoản nợ/cho vay.");

        if (request.Amount <= 0)
            throw new MyBudgetManagement.Application.Common.Exceptions.ValidationException("Số tiền phải lớn hơn 0.");

        var userBalance = await _uow.UserBalances.GetUserBalanceByUserIdAsync(userId)
            ?? throw new NotFoundException("Không tìm thấy số dư.");

        // Cập nhật AmountPaid và trạng thái
        debt.AmountPaid += request.Amount;

        if (debt.AmountPaid >= debt.Amount)
        {
            debt.AmountPaid = debt.Amount;
            debt.Status = PaymentStatus.Paid;
        }

        // Tạo transaction tương ứng
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            DebtAndLoanId = debt.Id,
            UserBalanceId = userBalance.Id,
            CategoryId = debt.CategoryId,
            Amount = request.Amount,
            Note = request.Note ?? string.Empty,
            Image = request.Image,
            Date = request.Date ?? DateTime.UtcNow,
            Created = DateTime.UtcNow,
            CreatedBy = userId
        };

        // Điều chỉnh số dư
        if (debt.IsDebt)
        {
            // Mình đã vay → giờ trả tiền → mất tiền → -Amount
            userBalance.Balance -= request.Amount;
        }
        else
        {
            // Mình đã cho vay → giờ được trả → nhận tiền → +Amount
            userBalance.Balance += request.Amount;
        }

        await _uow.Transactions.AddAsync(transaction);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}
