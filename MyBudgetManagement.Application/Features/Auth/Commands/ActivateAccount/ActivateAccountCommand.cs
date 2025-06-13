using MediatR;

namespace MyBudgetManagement.Application.Features.Auth.Commands.ActivateAccount;

public class ActivateAccountCommand : IRequest
{
    public string Token { get; set; }
}