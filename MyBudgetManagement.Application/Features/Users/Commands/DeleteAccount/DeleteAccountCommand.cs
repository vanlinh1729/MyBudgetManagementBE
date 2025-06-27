using MediatR;

namespace MyBudgetManagement.Application.Features.Users.Commands.DeleteAccount;

public class DeleteAccountCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
    public string Password { get; set; } = string.Empty;
    public string ConfirmationText { get; set; } = string.Empty; // User must type "DELETE" to confirm
} 