using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Features.Auth.Interfaces;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities.Users;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Application.Features.Auth.Commands.ResendActivationEmail;

public class ResendActivationEmailCommandHandler : IRequestHandler<ResendActivationEmailCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IEmailService _mailService;

    public ResendActivationEmailCommandHandler(IUnitOfWork uow, IJwtTokenService jwtTokenService, IEmailService mailService)
    {
        _uow = uow;
        _jwtTokenService = jwtTokenService;
        _mailService = mailService;
    }

    public async Task Handle(ResendActivationEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _uow.Users.GetUserByEmailAsync(request.Email);

        if (user == null)
            throw new NotFoundException("Người dùng không tồn tại.");

        if (user.Status == AccountStatus.Activated)
            throw new InvalidOperationException("Tài khoản đã được kích hoạt.");

        // Xóa hoặc revoke tất cả token ActivateToken cũ
        var oldTokens = await _uow.Tokens.Query()
            .Where(t => t.UserId == user.Id && t.Type == TokenType.ActivationToken && t.RevokedAt == null)
            .ToListAsync();

        foreach (var t in oldTokens)
            t.RevokedAt = DateTime.UtcNow;

        var newTokenValue = _jwtTokenService.GenerateRandomStringToken();
        var token = new Token
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenValue = newTokenValue,
            Type = TokenType.ActivationToken,
            CreatedAt = DateTime.UtcNow,
            ExpireAt = DateTime.UtcNow.AddDays(7)
        };

        await _uow.Tokens.AddAsync(token);
        await _uow.SaveChangesAsync(cancellationToken);
        await _mailService.SendActivateEmailAsync(user.Email, user.FullName, newTokenValue);
    }
}