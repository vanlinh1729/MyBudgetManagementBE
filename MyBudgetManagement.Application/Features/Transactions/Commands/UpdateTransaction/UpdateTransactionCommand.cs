using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MyBudgetManagement.Application.Features.Transactions.Commands.UpdateTransaction;

public class UpdateTransactionCommand: IRequest
{
    public Guid Id { get; set; }
    [Required]
    public decimal Amount { get; set; }
    [Required]
    public DateTime Date { get; set; }
    public string? Note { get; set; }
    public string? Image { get; set; }
    [Required]
    public Guid CategoryId { get; set; }
}