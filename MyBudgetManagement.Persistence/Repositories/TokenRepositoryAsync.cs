using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Features.Auth.Interfaces;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Domain.Interfaces.Repositories;
using MyBudgetManagement.Persistence.Context;

namespace MyBudgetManagement.Persistence.Repositories;


public class TokenRepositoryAsync : GenericRepositoryAsync<Token>, ITokenRepositoryAsync
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtTokenService _jwtProvider;

    public TokenRepositoryAsync(ApplicationDbContext context, IJwtTokenService jwtProvider) : base(context, context.Tokens)
    {
        _context = context;
        _jwtProvider = jwtProvider;
    }

    public async Task<Token> GetByToken(string token) =>
        await _context.Tokens.FirstOrDefaultAsync(rt => rt.TokenValue == token);

    public async Task SaveToken(Token token)
    {
        _context.Tokens.Add(token);
        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> RevokeToken(Guid userId, CancellationToken cancellationToken = default)
    {
        var oldRefreshToken = await _context.Tokens
            .FirstOrDefaultAsync(rt => rt.UserId == userId && rt.RevokedAt == null && rt.ExpireAt > DateTime.UtcNow, cancellationToken);

        if (oldRefreshToken == null)
        {
            return false; // Không có token hợp lệ để thu hồi
        }

        oldRefreshToken.RevokedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        return true; // Thu hồi thành công
    }

    public Task<string> RefreshToken(string refreshToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> RefreshAccessToken(string refreshTokenStr)
    {
        throw new NotImplementedException();
    }


    public async Task<string> RevokeAndGenerateNewRefreshTokenAsync(Guid userId)
    {
        await RevokeToken(userId);
        var refreshToken = new Token
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TokenValue = _jwtProvider.GenerateRefreshToken(),
            ExpireAt = DateTime.UtcNow.AddDays(7)
        };

        await SaveToken(refreshToken);
        return refreshToken.TokenValue;
    }

    // public async Task<string> RefreshAccessToken(string refreshTokenStr)
    // {
    //     var refreshToken = await GetByToken(refreshTokenStr);
    //     if (refreshToken == null || !refreshToken.IsActive)
    //         throw new UnauthorizedAccessException("Invalid refresh token");
    //
    //     if (refreshToken.IsExpired)
    //         throw new UnauthorizedAccessException("Expired refresh token");
    //     
    //     // Logic to create a new access token here
    //     var user = refreshToken.User ?? 
    //                await _context.Users
    //                    .Include(u => u.UserRoles)
    //                    .ThenInclude(ur => ur.Role)
    //                    .Include(u => u.UserBalance)
    //                    .SingleOrDefaultAsync(u => u.Id == refreshToken.UserId);
    //     var newAccessToken = _jwtProvider.GenerateToken(user); // Placeholder
    //
    //     return newAccessToken;
    // }
    // public async Task<string> RefreshToken(string refreshTokenStr)
    // {
    //     var token = await GetByToken(refreshTokenStr);
    //     if (token == null || !token.IsActive)
    //         throw new UnauthorizedAccessException("Invalid refresh token");
    //
    //     if (token.IsExpired)
    //         throw new UnauthorizedAccessException("Expired refresh token");
    //
    //     await RevokeToken(token.UserId);
    //     var refreshToken = new RefreshToken
    //     {
    //         Id = Guid.NewGuid(),
    //         UserId = token.UserId,
    //         Token = Guid.NewGuid().ToString(),
    //         ExpiryDate = DateTime.UtcNow.AddDays(7)
    //     };
    //
    //     await SaveToken(refreshToken);
    //     return refreshToken.Token;
    // }
}