using System.ComponentModel.DataAnnotations;
using MediatR;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Common.Interfaces;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities.Transactions;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Application.Features.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Guid>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public CreateTransactionCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        var category = await _uow.Categories.GetByIdAsync(request.CategoryId);
        if (category == null || category.UserId != userId)
            throw new NotFoundException("Không tìm thấy danh mục.");

        if (request.Amount <= 0)
            throw new MyBudgetManagement.Application.Common.Exceptions.ValidationException("Số tiền phải lớn hơn 0.");

        var userBalance = await _uow.UserBalances.GetUserBalanceByUserIdAsync(userId);
        if (userBalance == null)
            throw new NotFoundException("Không tìm thấy số dư người dùng.");

        Guid? debtAndLoanId = null;

        // Nếu là loại nợ/cho vay
        if (category.Type == CategoryType.DebtAndLoan)
        {
            if (!request.DebtAndLoanId.HasValue)
                                    throw new MyBudgetManagement.Application.Common.Exceptions.ValidationException("Phải cung cấp DebtAndLoanId cho giao dịch liên quan đến nợ.");

            var debt = await _uow.DebtAndLoans.GetByIdAsync(request.DebtAndLoanId.Value);
            if (debt == null)
                throw new NotFoundException("Không tìm thấy khoản nợ hoặc cho vay.");

            if (debt.AmountPaid + request.Amount > debt.Amount)
                                    throw new MyBudgetManagement.Application.Common.Exceptions.ValidationException("Số tiền trả/thu vượt quá khoản nợ.");

            debt.AmountPaid += request.Amount;
            if (debt.AmountPaid >= debt.Amount)
                debt.Status = PaymentStatus.Paid;

            // Cập nhật số dư dựa trên IsDebt
            if (debt.IsDebt)
                userBalance.Balance -= request.Amount; // Trả nợ
            else
                userBalance.Balance += request.Amount; // Thu nợ

            debtAndLoanId = debt.Id;
        }
        else
        {
            // Các loại transaction bình thường
            switch (category.Type)
            {
                case CategoryType.Income:
                    userBalance.Balance += request.Amount;
                    break;
                case CategoryType.Expense:
                    userBalance.Balance -= request.Amount;
                    break;
                default:
                    throw new InvalidOperationException("Loại danh mục không hợp lệ.");
            }
        }

        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            CategoryId = request.CategoryId,
            UserBalanceId = userBalance.Id,
            Amount = request.Amount,
            Date = request.Date,
            Note = request.Note ?? string.Empty,
            Image = request.Image,
            Created = DateTime.UtcNow,
            CreatedBy = userId,
            DebtAndLoanId = debtAndLoanId
        };

        await _uow.Transactions.AddAsync(transaction);
        await _uow.SaveChangesAsync(cancellationToken);

        return transaction.Id;
    }
}
