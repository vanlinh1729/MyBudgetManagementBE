using MediatR;
using MyBudgetManagement.Application.Features.DebtAndLoans.Dtos;

namespace MyBudgetManagement.Application.Features.DebtAndLoans.Queries.GetDebtAndLoanOverview;

public class GetDebtAndLoanOverviewQuery: IRequest<DebtAndLoanOverviewDto> { }