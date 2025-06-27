using MediatR;
using MyBudgetManagement.Application.Features.Users.Dtos;

namespace MyBudgetManagement.Application.Features.Users.Queries.GetUserProfile;

public class GetUserProfileQuery : IRequest<UserProfileDto>
{
    public Guid UserId { get; set; }
} 