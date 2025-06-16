using MediatR;

namespace MyBudgetManagement.Application.Features.DebtAndLoans.Commands.CreateDebtAndLoan;

public class CreateDebtAndLoanCommand : IRequest<Guid>
{
    public Guid DebtContactId { get; set; }
    public Guid CategoryId { get; set; }
    public bool IsDebt { get; set; }
    public decimal Amount { get; set; }
    public string? Image { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? Note { get; set; }   
}