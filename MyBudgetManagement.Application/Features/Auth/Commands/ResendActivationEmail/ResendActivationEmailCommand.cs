using MediatR;

namespace MyBudgetManagement.Application.Features.Auth.Commands.ResendActivationEmail;

public class ResendActivationEmailCommand : IRequest
{
    public string Email { get; set; }

}