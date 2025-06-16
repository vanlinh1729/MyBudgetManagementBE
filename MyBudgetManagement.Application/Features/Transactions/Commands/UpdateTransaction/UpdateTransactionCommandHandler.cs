using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Common.Interfaces;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Application.Features.Transactions.Commands.UpdateTransaction;

public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public UpdateTransactionCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        var transaction = await _uow.Transactions
            .Query()
            .Include(t => t.UserBalance)
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == request.Id && t.UserBalance.UserId == userId, cancellationToken);

        if (transaction == null)
            throw new NotFoundException("Không tìm thấy giao dịch.");

        var category = await _uow.Categories.GetByIdAsync(request.CategoryId);
        if (category == null || category.UserId != userId)
            throw new NotFoundException("Không tìm thấy danh mục.");

        var userBalance = transaction.UserBalance;

        // Reverse balance từ giao dịch cũ
        if (transaction.Category.Type != CategoryType.DebtAndLoan)
        {
            userBalance.Balance += transaction.Category.Type == CategoryType.Income
                ? -transaction.Amount
                : transaction.Amount;
        }

        // Cập nhật thông tin
        transaction.Amount = request.Amount;
        transaction.Date = request.Date;
        transaction.Note = request.Note ?? "";
        transaction.Image = request.Image;
        transaction.CategoryId = request.CategoryId;

        // Áp dụng lại balance theo giao dịch mới
        if (category.Type != CategoryType.DebtAndLoan)
        {
            userBalance.Balance += category.Type == CategoryType.Income
                ? request.Amount
                : -request.Amount;
        }

        // Nếu là DebtAndLoan thì cập nhật lại AmountPaid
        if (transaction.DebtAndLoanId.HasValue)
        {
            var debt = await _uow.DebtAndLoans.GetByIdAsync(transaction.DebtAndLoanId.Value);
            if (debt == null)
                throw new NotFoundException("Không tìm thấy khoản nợ/cho vay.");

            debt.AmountPaid -= transaction.Amount; // revert cũ
            debt.AmountPaid += request.Amount;     // cộng mới

            if (debt.AmountPaid >= debt.Amount)
                debt.Status = PaymentStatus.Paid;
            else
                debt.Status = PaymentStatus.Unpaid;
        }

        await _uow.SaveChangesAsync(cancellationToken);
    }
}