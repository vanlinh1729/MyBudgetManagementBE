using MediatR;
using MyBudgetManagement.Application.Features.DebtAndLoans.Dtos;

namespace MyBudgetManagement.Application.Features.DebtAndLoans.Queries.GetDebtAndLoanList;

public class GetDebtAndLoanListQuery : IRequest <List<DebtAndLoanDto>>
{
    
}