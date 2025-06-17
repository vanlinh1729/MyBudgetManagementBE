using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Application.Features.DebtAndLoans.Dtos;

public class DebtAndLoanSummaryDto
{
    public Guid Id { get; set; }
    public bool IsDebt { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal RemainingAmount { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentStatus Status { get; set; }
}