using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Common.Interfaces;
using MyBudgetManagement.Application.Features.DebtAndLoans.Dtos;
using MyBudgetManagement.Application.Interfaces;

namespace MyBudgetManagement.Application.Features.DebtAndLoans.Queries.GetDebtAndLoanList;

public class GetDebtAndLoanListQueryHandler : IRequestHandler<GetDebtAndLoanListQuery, List<DebtAndLoanDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public GetDebtAndLoanListQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<List<DebtAndLoanDto>> Handle(GetDebtAndLoanListQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        var debts = await _uow.DebtAndLoans.Query()
            .Where(d => d.CreatedBy == userId)
            .Include(d => d.DebtAndLoanContact)
            .Include(d => d.Category)
            .ToListAsync(cancellationToken);


        return debts.Select(d => new DebtAndLoanDto
        {
            Id = d.Id,
            DebtContactId = d.DebtContactId,
            ContactName = d.DebtAndLoanContact.Name,
            CategoryId = d.CategoryId,
            CategoryName = d.Category.Name,
            IsDebt = d.IsDebt,
            Amount = d.Amount,
            AmountPaid = d.AmountPaid,
            Status = d.Status,
            StartDate = d.StartDate,
            PaymentDate = d.PaymentDate,
            Note = d.Note,
            Image = d.Image
        }).ToList();
    }
}
