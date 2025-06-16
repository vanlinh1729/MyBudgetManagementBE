using System.ComponentModel.DataAnnotations;
using MediatR;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Common.Interfaces;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities.Debts;
using MyBudgetManagement.Domain.Entities.Transactions;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Application.Features.DebtAndLoans.Commands.CreateDebtAndLoan;

public class CreateDebtAndLoanCommandHandler : IRequestHandler<CreateDebtAndLoanCommand, Guid>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public CreateDebtAndLoanCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(CreateDebtAndLoanCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        var category = await _uow.Categories.GetByIdAsync(request.CategoryId);
        if (category == null || category.Type != CategoryType.DebtAndLoan || category.UserId != userId)
            throw new NotFoundException("Danh mục không hợp lệ.");

        if (request.Amount <= 0)
            throw new ValidationException("Số tiền phải lớn hơn 0.");

        var userBalance = await _uow.UserBalances.GetUserBalanceByUserIdAsync(userId);
        if (userBalance == null)
            throw new NotFoundException("Không tìm thấy số dư.");

        // Tạo DebtAndLoan gốc
        var debt = new DebtAndLoan
        {
            Id = Guid.NewGuid(),
            DebtContactId = request.DebtContactId,
            CategoryId = request.CategoryId,
            IsDebt = request.IsDebt,
            Amount = request.Amount,
            AmountPaid = 0,
            StartDate = request.StartDate,
            PaymentDate = request.PaymentDate,
            Status = PaymentStatus.Unpaid,
            Note = request.Note ?? string.Empty,
            Image = request.Image,
            Created = DateTime.UtcNow,
            CreatedBy = userId
        };
        await _uow.DebtAndLoans.AddAsync(debt);

        // Xác định loại Transaction và số dư
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            CategoryId = request.CategoryId,
            UserBalanceId = userBalance.Id,
            Amount = request.Amount,
            Date = request.StartDate,
            Note = request.Note ?? string.Empty,
            Image = request.Image,
            Created = DateTime.UtcNow,
            CreatedBy = userId,
            DebtAndLoanId = debt.Id
        };

        if (request.IsDebt)
        {
            // Mình đi vay → nhận tiền → cộng tiền
            userBalance.Balance += request.Amount;
        }
        else
        {
            // Mình cho vay → mất tiền → trừ tiền
            userBalance.Balance -= request.Amount;
        }

        await _uow.Transactions.AddAsync(transaction);
        await _uow.SaveChangesAsync(cancellationToken);

        return debt.Id;
    }
}
