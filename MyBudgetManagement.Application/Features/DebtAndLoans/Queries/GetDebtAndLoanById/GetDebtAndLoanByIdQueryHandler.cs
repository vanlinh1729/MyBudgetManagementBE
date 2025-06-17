using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Common.Interfaces;
using MyBudgetManagement.Application.Features.DebtAndLoans.Dtos;
using MyBudgetManagement.Application.Interfaces;

namespace MyBudgetManagement.Application.Features.DebtAndLoans.Queries.GetDebtAndLoanById;

public class GetDebtAndLoanByIdQueryHandler : IRequestHandler<GetDebtAndLoanByIdQuery, DebtAndLoanDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public GetDebtAndLoanByIdQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<DebtAndLoanDto> Handle(GetDebtAndLoanByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        var debt = await _uow.DebtAndLoans
            .Query()
            .Where(d => d.Id == request.Id && d.CreatedBy == userId)
            .Include(d => d.DebtAndLoanContact)
            .Include(d => d.Category)
            .FirstOrDefaultAsync();

        if (debt == null)
            throw new NotFoundException("Không tìm thấy khoản vay/nợ.");

        return new DebtAndLoanDto
        {
            Id = debt.Id,
            DebtContactId = debt.DebtContactId,
            ContactName = debt.DebtAndLoanContact.Name,
            CategoryId = debt.CategoryId,
            CategoryName = debt.Category.Name,
            IsDebt = debt.IsDebt,
            Amount = debt.Amount,
            AmountPaid = debt.AmountPaid,
            Status = debt.Status,
            StartDate = debt.StartDate,
            PaymentDate = debt.PaymentDate,
            Note = debt.Note,
            Image = debt.Image
        };
    }
}
