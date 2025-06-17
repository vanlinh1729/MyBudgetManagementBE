using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MyBudgetManagement.Application.Features.DebtAndLoans.Commands.PayDebtAndLoan;

public class PayDebtAndLoanCommand: IRequest
{
    [Required]
    public Guid DebtAndLoanId { get; set; }

    [Required]
    [Range(1, double.MaxValue, ErrorMessage = "Số tiền phải lớn hơn 0.")]
    public decimal Amount { get; set; }

    public string? Note { get; set; }
    public string? Image { get; set; }
    public DateTime? Date { get; set; }
}