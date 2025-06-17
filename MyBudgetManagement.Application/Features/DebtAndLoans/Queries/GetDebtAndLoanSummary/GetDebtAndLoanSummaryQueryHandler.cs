using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Common.Interfaces;
using MyBudgetManagement.Application.Features.DebtAndLoans.Dtos;
using MyBudgetManagement.Application.Interfaces;

namespace MyBudgetManagement.Application.Features.DebtAndLoans.Queries.GetDebtAndLoanSummary;

public class GetDebtAndLoanSummaryQueryHandler : IRequestHandler<GetDebtAndLoanSummaryQuery, DebtAndLoanSummaryDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public GetDebtAndLoanSummaryQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<DebtAndLoanSummaryDto> Handle(GetDebtAndLoanSummaryQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        var entity = await _uow.DebtAndLoans.Query()
            .Where(d => d.Id == request.Id && d.CreatedBy == userId)
            .Select(d => new DebtAndLoanSummaryDto
            {
                Id = d.Id,
                IsDebt = d.IsDebt,
                TotalAmount = d.Amount,
                AmountPaid = d.AmountPaid,
                RemainingAmount = d.Amount - d.AmountPaid,
                PaymentDate = d.PaymentDate,
                Status = d.Status
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
            throw new NotFoundException("Không tìm thấy khoản nợ.");

        return entity;
    }
}