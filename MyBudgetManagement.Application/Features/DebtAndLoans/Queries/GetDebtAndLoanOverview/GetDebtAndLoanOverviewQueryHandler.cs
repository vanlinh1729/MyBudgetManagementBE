using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Common.Interfaces;
using MyBudgetManagement.Application.Features.DebtAndLoans.Dtos;
using MyBudgetManagement.Application.Interfaces;

namespace MyBudgetManagement.Application.Features.DebtAndLoans.Queries.GetDebtAndLoanOverview;

public class GetDebtAndLoanOverviewQueryHandler : IRequestHandler<GetDebtAndLoanOverviewQuery, DebtAndLoanOverviewDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public GetDebtAndLoanOverviewQueryHandler(IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<DebtAndLoanOverviewDto> Handle(GetDebtAndLoanOverviewQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        var query = _uow.DebtAndLoans.Query()
            .Where(x => x.CreatedBy == userId);

        var debtCount = await query.CountAsync(x => x.IsDebt, cancellationToken);
        var loanCount = await query.CountAsync(x => !x.IsDebt, cancellationToken);

        var totalDebtAmount = await query.Where(x => x.IsDebt).SumAsync(x => (decimal?)x.Amount, cancellationToken) ?? 0;
        var totalLoanAmount = await query.Where(x => !x.IsDebt).SumAsync(x => (decimal?)x.Amount, cancellationToken) ?? 0;

        var totalDebtPaid = await query.Where(x => x.IsDebt).SumAsync(x => (decimal?)x.AmountPaid, cancellationToken) ?? 0;
        var totalLoanCollected = await query.Where(x => !x.IsDebt).SumAsync(x => (decimal?)x.AmountPaid, cancellationToken) ?? 0;

        return new DebtAndLoanOverviewDto
        {
            DebtCount = debtCount,
            LoanCount = loanCount,
            TotalDebtAmount = totalDebtAmount,
            TotalLoanAmount = totalLoanAmount,
            TotalDebtPaid = totalDebtPaid,
            TotalLoanCollected = totalLoanCollected
        };
    }
}
