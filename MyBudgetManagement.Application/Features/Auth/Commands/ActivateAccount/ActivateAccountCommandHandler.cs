using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Application.Features.Auth.Commands.ActivateAccount;

public class ActivateAccountCommandHandler : IRequestHandler<ActivateAccountCommand>
{
    private readonly IApplicationDbContext _context;

    public ActivateAccountCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(ActivateAccountCommand request, CancellationToken cancellationToken)
    {
        var token = await _context.Tokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t =>
                    t.TokenValue == request.Token &&
                    t.Type == TokenType.ActivationToken &&
                    t.RevokedAt == null &&
                    t.ExpireAt > DateTime.UtcNow,
                cancellationToken);

        if (token == null)
            throw new NotFoundException("Token không hợp lệ hoặc đã hết hạn");

        var user = token.User;
        if (user == null)
            throw new NotFoundException("Không tìm thấy người dùng");

        if (user.Status == AccountStatus.Activated)
            throw new InvalidOperationException("Tài khoản đã được kích hoạt");

        user.Status = AccountStatus.Activated;
        token.RevokedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        
    }
}