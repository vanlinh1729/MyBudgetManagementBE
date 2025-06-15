using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Entities.Users;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Application.Features.Auth.Commands.ActivateAccount;

public class ActivateAccountCommandHandler : IRequestHandler<ActivateAccountCommand>
{
    private readonly IUnitOfWork _uow;

    public ActivateAccountCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task Handle(ActivateAccountCommand request, CancellationToken cancellationToken)
    {
        await _uow.BeginTransactionAsync();
        try
        {
            var token = await _uow.Tokens.GetByToken(request.Token);

            if (token == null ||
                token.Type != TokenType.ActivationToken ||
                token.RevokedAt != null ||
                token.ExpireAt < DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Token không hợp lệ hoặc đã hết hạn.");
            }

            var user = await _uow.Users.GetByIdAsync(token.UserId);
            if (user == null)
                throw new NotFoundException("Không tìm thấy người dùng.");

            if (user.Status == AccountStatus.Activated)
                throw new InvalidOperationException("Tài khoản đã được kích hoạt.");

            user.Status = AccountStatus.Activated;
            token.RevokedAt = DateTime.UtcNow;

            var userBalance = new UserBalance
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Balance = 0,
                Created = DateTime.UtcNow,
                CreatedBy = user.Id
            };

            await _uow.UserBalances.AddAsync(userBalance);
            //gasn role user mac dinh sau khi activate account
            await _uow.Users.AssignRoleAsync(user.Id, "User", user.Id, cancellationToken);

            await _uow.SaveChangesAsync(cancellationToken);
            await _uow.CommitAsync();
        }
        catch
        {
            await _uow.RollbackAsync();
            throw;
        }
    }
}
