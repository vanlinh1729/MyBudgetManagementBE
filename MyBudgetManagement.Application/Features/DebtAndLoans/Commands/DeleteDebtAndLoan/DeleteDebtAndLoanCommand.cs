using MediatR;

namespace MyBudgetManagement.Application.Features.DebtAndLoans.Commands.DeleteDebtAndLoan;

public class DeleteDebtAndLoanCommand: IRequest
{
    public Guid Id { get; set; }
}