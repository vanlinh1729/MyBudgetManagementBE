using System.Security.Claims;

namespace MyBudgetManagement.Application.Features.Auth.Interfaces;

public interface IJwtTokenService
{
    string GenerateAccessToken(Guid userId, string email, IList<string> roles);
    string GenerateRefreshToken();
    string GenerateRandomStringToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}