using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Features.Auth.Interfaces;
using MyBudgetManagement.Application.Interfaces;

namespace MyBudgetManagement.Application.Features.Users.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
{
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher _passwordHasher;

    public ChangePasswordCommandHandler(IUnitOfWork uow, IPasswordHasher passwordHasher)
    {
        _uow = uow;
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        // Validate password confirmation
        if (request.NewPassword != request.ConfirmPassword)
        {
            throw new ArgumentException("Mật khẩu xác nhận không khớp");
        }

        // Validate password strength (basic validation)
        if (string.IsNullOrWhiteSpace(request.NewPassword) || request.NewPassword.Length < 6)
        {
            throw new ArgumentException("Mật khẩu mới phải có ít nhất 6 ký tự");
        }

        // Get user
        var user = await _uow.Users.Query()
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException($"User with ID {request.UserId} not found");
        }

        // Verify current password
        if (!_passwordHasher.Verify(request.CurrentPassword, user.PasswordHash))
        {
            throw new UnauthorizedException("Mật khẩu hiện tại không đúng");
        }

        // Hash new password
        var newPasswordHash = _passwordHasher.Hash(request.NewPassword);

        // Update user password
        user.PasswordHash = newPasswordHash;
        user.LastChangePassword = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;

        _uow.Users.Update(user);
        await _uow.SaveChangesAsync();

        return true;
    }
} 