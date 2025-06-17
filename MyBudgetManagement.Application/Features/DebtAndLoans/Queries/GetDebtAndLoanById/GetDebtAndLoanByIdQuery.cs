using MediatR;
using MyBudgetManagement.Application.Features.DebtAndLoans.Dtos;

namespace MyBudgetManagement.Application.Features.DebtAndLoans.Queries.GetDebtAndLoanById;

public class GetDebtAndLoanByIdQuery : IRequest<DebtAndLoanDto>
{
    public Guid Id { get; set; }
}