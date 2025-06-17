using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Common.Interfaces;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Application.Features.DebtAndLoans.Commands.UpdateDebtAndLoan;

public class UpdateDebtAndLoanCommandHandler : IRequestHandler<UpdateDebtAndLoanCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public UpdateDebtAndLoanCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task Handle(UpdateDebtAndLoanCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;
        var debt = await _uow.DebtAndLoans.GetByIdAsync(request.Id)
                   ?? throw new NotFoundException("Không tìm thấy khoản nợ/vay.");

        if (debt.CreatedBy != userId)
            throw new UnauthorizedAccessException("Không có quyền cập nhật.");

        var category = await _uow.Categories.GetByIdAsync(request.CategoryId)
                       ?? throw new NotFoundException("Danh mục không tồn tại.");

        if (category.Type != CategoryType.DebtAndLoan || category.UserId != userId)
            throw new ValidationException("Danh mục không hợp lệ.");

        var userBalance = await _uow.UserBalances.GetUserBalanceByUserIdAsync(userId)
                          ?? throw new NotFoundException("Không tìm thấy số dư.");

        var transaction = await _uow.Transactions.Query().FirstOrDefaultAsync(x => x.DebtAndLoanId == debt.Id);
        if (transaction == null)
            throw new NotFoundException("Không tìm thấy giao dịch liên quan.");

        // ➤ Rollback old balance
        if (debt.IsDebt)
            userBalance.Balance -= debt.Amount;
        else
            userBalance.Balance += debt.Amount;

        // ➤ Apply new balance
        if (request.IsDebt)
            userBalance.Balance += request.Amount;
        else
            userBalance.Balance -= request.Amount;

        // ➤ Update DebtAndLoan
        debt.CategoryId = request.CategoryId;
        debt.DebtContactId = request.DebtContactId;
        debt.IsDebt = request.IsDebt;
        debt.Amount = request.Amount;
        debt.PaymentDate = request.PaymentDate;
        debt.Note = request.Note ?? string.Empty;
        debt.Image = request.Image;
        debt.StartDate = request.StartDate;
        debt.UpdatedAt = DateTime.UtcNow;
        debt.UpdatedBy = userId;

        // ➤ Update Transaction
        transaction.CategoryId = request.CategoryId;
        transaction.Amount = request.Amount;
        transaction.Date = request.StartDate;
        transaction.Note = request.Note ?? string.Empty;
        transaction.Image = request.Image;
        transaction.UpdatedAt = DateTime.UtcNow;
        transaction.UpdatedBy = userId;

        await _uow.SaveChangesAsync(cancellationToken);
    }
}
