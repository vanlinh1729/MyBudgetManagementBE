using MediatR;
using Microsoft.AspNetCore.Http;
using MyBudgetManagement.Application.Features.Users.Dtos;

namespace MyBudgetManagement.Application.Features.Users.Commands.UploadAvatar;

public class UploadAvatarCommand : IRequest<UserProfileDto>
{
    public Guid UserId { get; set; }
    public IFormFile File { get; set; } = null!;
} 