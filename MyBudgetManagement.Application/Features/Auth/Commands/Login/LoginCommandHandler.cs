using System.Security.Claims;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Features.Auth.Dtos;
using MyBudgetManagement.Application.Features.Auth.Interfaces;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities.Users;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IUnitOfWork _uow;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordHasher _passwordHasher;

    public LoginCommandHandler(IUnitOfWork uow, IJwtTokenService jwtTokenService, IPasswordHasher passwordHasher)
    {
        _uow = uow;
        _jwtTokenService = jwtTokenService;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // 1. Find user by email
        var user = await _uow.Users.GetUserByEmailAsync(request.Email);
        if (user == null)
        {
            throw new UnauthorizedException("Email hoặc mật khẩu không đúng");
        }

        // 2. Verify password
        if (!_passwordHasher.Verify(request.Password,user.PasswordHash))
        {
            throw new UnauthorizedException("Email hoặc mật khẩu không đúng");
        }

        // 3. Check account status
        if (user.Status == AccountStatus.Pending)
        {
            throw new AccountNotActivated("Tài khoản chưa được kích hoạt");
        }

        // 4. Get user roles
        var roles = await _uow.UserRoles.Query()
            .Where(ur => ur.UserId == user.Id)
            .Include(ur => ur.Role)
            .Select(ur => ur.Role.Name)
            .ToListAsync();

        // 5. Generate tokens with roles
        var accessToken = _jwtTokenService.GenerateAccessToken(user.Id, user.Email, roles);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();

        // 6. Save refresh token
        var tokenEntity = new Token
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenValue = refreshToken,
            Type = TokenType.RefreshToken,
            CreatedAt = DateTime.UtcNow,
            ExpireAt = DateTime.UtcNow.AddDays(7)
        };

        await _uow.Tokens.AddAsync(tokenEntity);
        await _uow.SaveChangesAsync();

        // 7. Return response
        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Roles = roles,
            // ExpiresAt = _jwtTokenService.GetAccessTokenExpiration()
        };
    }
}