using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Application.Features.Auth.Commands.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, bool>
{
    private readonly IUnitOfWork _uow;

    public LogoutCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        // Find and remove the refresh token
        var tokenEntity = await _uow.Tokens.Query()
            .FirstOrDefaultAsync(t => t.TokenValue == request.RefreshToken && 
                                    t.Type == TokenType.RefreshToken, 
                                    cancellationToken);

        if (tokenEntity != null)
        {
            _uow.Tokens.Remove(tokenEntity);
            await _uow.SaveChangesAsync();
        }

        return true;
    }
} 