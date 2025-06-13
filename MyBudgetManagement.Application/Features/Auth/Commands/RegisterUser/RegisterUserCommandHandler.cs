using MediatR;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Features.Auth.Dtos;
using MyBudgetManagement.Application.Features.Auth.Interfaces;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Application.Features.Auth.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserCommandHandler(IApplicationDbContext context, IJwtTokenService jwtTokenService, IPasswordHasher passwordHasher)
    {
        _context = context;
        _jwtTokenService = jwtTokenService;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (_context.Users.Any(x => x.Email == request.Email))
            throw new ConflictException("Email already exists.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            FullName = request.FullName,
            Status = Domain.Enums.AccountStatus.Activated,
            LastChangePassword = DateTime.UtcNow
        };

        user.PasswordHash = _passwordHasher.Hash(request.Password);

        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var accessToken = _jwtTokenService.GenerateAccessToken(user.Id, user.Email, new List<string>());
        var refreshToken = _jwtTokenService.GenerateRefreshToken();

        var token = new Token
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenValue = refreshToken,
            Type = TokenType.RefreshToken,
            CreatedAt = DateTime.UtcNow,
            ExpireAt = DateTime.UtcNow.AddDays(7)
        };

        await _context.Tokens.AddAsync(token);
        await _context.SaveChangesAsync(CancellationToken.None);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

    }
}