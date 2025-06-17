using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Application.Features.DebtAndLoans.Dtos;

public class DebtAndLoanDto
{
    public Guid Id { get; set; }

    public Guid DebtContactId { get; set; }
    public string ContactName { get; set; }

    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string CategoryIcon { get; set; }

    public bool IsDebt { get; set; }

    public decimal Amount { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal RemainingAmount => Amount - AmountPaid;

    public DateTime StartDate { get; set; }
    public DateTime PaymentDate { get; set; }

    public PaymentStatus Status { get; set; }

    public string? Image { get; set; }
    public string Note { get; set; }

    public int TransactionCount { get; set; }
}