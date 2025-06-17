using MediatR;
using MyBudgetManagement.Application.Features.DebtAndLoans.Dtos;

namespace MyBudgetManagement.Application.Features.DebtAndLoans.Queries.GetDebtAndLoanSummary;

public class GetDebtAndLoanSummaryQuery: IRequest<DebtAndLoanSummaryDto>
{
    public Guid Id { get; set; }

    public GetDebtAndLoanSummaryQuery(Guid id)
    {
        Id = id;
    }
}