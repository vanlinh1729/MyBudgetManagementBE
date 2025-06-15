using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Entities.Users;

namespace MyBudgetManagement.Domain.Interfaces.Repositories;

public interface ITokenRepositoryAsync : IGenericRepositoryAsync<Token>
{
    Task<Token> GetByToken(string token);
    Task SaveToken(Token token);
    Task<bool> RevokeToken(Guid userId, CancellationToken cancellationToken = default);
    Task<string> RefreshToken(string refreshToken);
    Task<string> RefreshAccessToken(string refreshTokenStr);
    Task<string> RevokeAndGenerateNewRefreshTokenAsync(Guid userId);
}