using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
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

            if (token == null)
                throw new NotFoundException("Token không hợp lệ hoặc đã hết hạn");

            var user = token.User;
            if (user == null)
                throw new NotFoundException("Không tìm thấy người dùng");

            if (user.Status == AccountStatus.Activated)
                throw new InvalidOperationException("Tài khoản đã được kích hoạt");

            user.Status = AccountStatus.Activated;
            token.RevokedAt = DateTime.UtcNow;
            var userBalance = new UserBalance
            {
                Balance = 0,
                Created = DateTime.Now,
                CreatedBy = user.Id,
                Id = Guid.NewGuid(),
                UserId = user.Id
            };
            await _uow.UserBalances.AddAsync(userBalance);
        
            await _uow.SaveChangesAsync(cancellationToken);
            await _uow.CommitAsync();
        }
        catch (Exception e)
        {
            await _uow.RollbackAsync();
            throw;
        }
      
        
    }
}