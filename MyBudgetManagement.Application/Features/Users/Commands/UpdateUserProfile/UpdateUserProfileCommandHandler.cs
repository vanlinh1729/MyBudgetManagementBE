using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Features.Users.Dtos;
using MyBudgetManagement.Application.Interfaces;

namespace MyBudgetManagement.Application.Features.Users.Commands.UpdateUserProfile;

public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, UserProfileDto>
{
    private readonly IUnitOfWork _uow;

    public UpdateUserProfileCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<UserProfileDto> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        // Get user with related data
        var user = await _uow.Users.Query()
            .Include(u => u.UserBalance)
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException($"User with ID {request.UserId} not found");
        }

        // Update user properties
        user.FullName = request.FullName;
        user.Gender = request.Gender;
        user.DateOfBirth = request.DateOfBirth;
        user.PhoneNumber = request.PhoneNumber;
        user.Currency = request.Currency;
        user.UpdatedAt = DateTime.UtcNow;

        // Update user
        _uow.Users.Update(user);
        await _uow.SaveChangesAsync();

        // Return updated profile
        return new UserProfileDto
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Avatar = user.Avatar,
            Gender = user.Gender,
            DateOfBirth = user.DateOfBirth,
            PhoneNumber = user.PhoneNumber,
            Status = user.Status,
            Currency = user.Currency,
            LastChangePassword = user.LastChangePassword,
            CreatedAt = user.Created,
            LastModifiedAt = user.UpdatedAt,
            CurrentBalance = user.UserBalance?.Balance ?? 0,
            Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList()
        };
    }
} 