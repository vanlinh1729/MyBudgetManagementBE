using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Features.Auth.Interfaces;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Application.Features.Users.Commands.DeleteAccount;

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, bool>
{
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher _passwordHasher;

    public DeleteAccountCommandHandler(IUnitOfWork uow, IPasswordHasher passwordHasher)
    {
        _uow = uow;
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        // Validate confirmation text
        if (request.ConfirmationText != "DELETE")
        {
            throw new ArgumentException("Bạn phải gõ 'DELETE' để xác nhận xóa tài khoản");
        }

        // Get user
        var user = await _uow.Users.Query()
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException($"User with ID {request.UserId} not found");
        }

        // Verify password
        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedException("Mật khẩu không đúng");
        }

        // For safety, we'll mark account as deleted instead of hard delete
        // This allows for account recovery and maintains data integrity
        user.Status = AccountStatus.Deleted;
        user.UpdatedAt = DateTime.UtcNow;
        
        // Optionally anonymize email to prevent login
        user.Email = $"deleted_user_{user.Id}@deleted.local";

        _uow.Users.Update(user);

        // Remove all active tokens to force logout
        var userTokens = await _uow.Tokens.Query()
            .Where(t => t.UserId == request.UserId)
            .ToListAsync(cancellationToken);

        foreach (var token in userTokens)
        {
            _uow.Tokens.Remove(token);
        }

        await _uow.SaveChangesAsync();

        return true;
    }
} 