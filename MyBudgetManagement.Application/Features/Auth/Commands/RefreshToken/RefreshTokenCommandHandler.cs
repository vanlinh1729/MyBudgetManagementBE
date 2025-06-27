using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Features.Auth.Dtos;
using MyBudgetManagement.Application.Features.Auth.Interfaces;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities.Users;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponse>
{
    private readonly IUnitOfWork _uow;
    private readonly IJwtTokenService _jwtTokenService;

    public RefreshTokenCommandHandler(IUnitOfWork uow, IJwtTokenService jwtTokenService)
    {
        _uow = uow;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // 1. Find and validate refresh token
        var tokenEntity = await _uow.Tokens.Query()
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.TokenValue == request.RefreshToken && 
                                    t.Type == TokenType.RefreshToken, 
                                    cancellationToken);

        if (tokenEntity == null)
        {
            throw new UnauthorizedException("Refresh token không hợp lệ");
        }

        // 2. Check if token is expired
        if (tokenEntity.ExpireAt <= DateTime.UtcNow)
        {
            // Remove expired token
            _uow.Tokens.Remove(tokenEntity);
            await _uow.SaveChangesAsync();
            throw new UnauthorizedException("Refresh token đã hết hạn");
        }

        // 3. Check if user is still active
        if (tokenEntity.User.Status != AccountStatus.Activated)
        {
            throw new UnauthorizedException("Tài khoản không còn hoạt động");
        }

        // 4. Get user roles
        var roles = await _uow.UserRoles.Query()
            .Where(ur => ur.UserId == tokenEntity.UserId)
            .Include(ur => ur.Role)
            .Select(ur => ur.Role.Name)
            .ToListAsync(cancellationToken);

        // 5. Generate new tokens
        var newAccessToken = _jwtTokenService.GenerateAccessToken(
            tokenEntity.UserId, 
            tokenEntity.User.Email, 
            roles
        );
        var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

        // 6. Replace old refresh token with new one
        tokenEntity.TokenValue = newRefreshToken;
        tokenEntity.CreatedAt = DateTime.UtcNow;
        tokenEntity.ExpireAt = DateTime.UtcNow.AddDays(7);

        _uow.Tokens.Update(tokenEntity);
        await _uow.SaveChangesAsync();

        // 7. Return new tokens
        return new AuthResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            Roles = roles
        };
    }
} 