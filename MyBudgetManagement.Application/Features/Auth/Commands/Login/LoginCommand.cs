using MediatR;
using MyBudgetManagement.Application.Features.Auth.Dtos;

namespace MyBudgetManagement.Application.Features.Auth.Commands.Login;

public class LoginCommand : IRequest<AuthResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }    
}