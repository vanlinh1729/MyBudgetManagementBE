using MediatR;

namespace MyBudgetManagement.Application.Features.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommand : IRequest<Guid>
{
    public Guid CategoryId { get; set; }
    public Guid? DebtAndLoanId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? Note { get; set; }
    public string? Image { get; set; }
}