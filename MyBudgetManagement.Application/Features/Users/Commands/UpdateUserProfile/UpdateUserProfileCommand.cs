using MediatR;
using MyBudgetManagement.Application.Features.Users.Dtos;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Application.Features.Users.Commands.UpdateUserProfile;

public class UpdateUserProfileCommand : IRequest<UserProfileDto>
{
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? PhoneNumber { get; set; }
    public Currencies Currency { get; set; }
} 