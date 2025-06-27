using MediatR;
using MyBudgetManagement.Application.Features.Auth.Dtos;

namespace MyBudgetManagement.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<AuthResponse>
{
    public string RefreshToken { get; set; } = string.Empty;
} 