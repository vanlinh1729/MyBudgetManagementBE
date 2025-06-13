using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MyBudgetManagement.Application.Common.Interfaces;

namespace MyBudgetManagement.Infrastructure.AuthService;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId =>
        Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)!);

}