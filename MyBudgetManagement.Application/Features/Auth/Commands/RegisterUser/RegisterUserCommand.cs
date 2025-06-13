using MediatR;
using MyBudgetManagement.Application.Features.Auth.Dtos;

namespace MyBudgetManagement.Application.Features.Auth.Commands.RegisterUser;

public class RegisterUserCommand: IRequest<Guid>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
}