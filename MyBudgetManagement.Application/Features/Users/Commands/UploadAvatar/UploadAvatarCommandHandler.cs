using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Features.Users.Dtos;
using MyBudgetManagement.Application.Interfaces;

namespace MyBudgetManagement.Application.Features.Users.Commands.UploadAvatar;

public class UploadAvatarCommandHandler : IRequestHandler<UploadAvatarCommand, UserProfileDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IFileStorageService _fileStorageService;
    private readonly ILogger<UploadAvatarCommandHandler> _logger;

    public UploadAvatarCommandHandler(
        IUnitOfWork uow,
        IFileStorageService fileStorageService,
        ILogger<UploadAvatarCommandHandler> logger)
    {
        _uow = uow;
        _fileStorageService = fileStorageService;
        _logger = logger;
    }

    public async Task<UserProfileDto> Handle(UploadAvatarCommand request, CancellationToken cancellationToken)
    {
        // Validate file
        if (request.File == null || request.File.Length == 0)
        {
            throw new ValidationException("No file provided for avatar upload");
        }

        // Validate file type (only images)
        var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };
        if (!allowedTypes.Contains(request.File.ContentType.ToLower()))
        {
            throw new ValidationException("Only image files (JPEG, PNG, GIF, WebP) are allowed for avatar");
        }

        // Validate file size (max 2MB for avatar)
        const long maxFileSize = 2 * 1024 * 1024; // 2MB
        if (request.File.Length > maxFileSize)
        {
            throw new ValidationException("Avatar file size must not exceed 2MB");
        }

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

        try
        {
            _logger.LogInformation("Starting avatar upload for user: {UserId}", request.UserId);

            // Delete old avatar if exists
            if (!string.IsNullOrEmpty(user.Avatar))
            {
                try
                {
                    await _fileStorageService.DeleteFileAsync(user.Avatar);
                    _logger.LogInformation("Old avatar deleted for user: {UserId}", request.UserId);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to delete old avatar for user: {UserId}", request.UserId);
                    // Continue with upload even if old avatar deletion fails
                }
            }

            // Upload new avatar
            using var stream = request.File.OpenReadStream();
            var fileName = request.File.FileName;
            var fileExtension = Path.GetExtension(fileName);
            var uniqueFileName = $"avatar_{request.UserId}_{Guid.NewGuid()}{fileExtension}";

            // Upload to "avatars" folder with specific dimensions for avatar
            var avatarUrl = await _fileStorageService.UploadFileAsync(
                stream,
                uniqueFileName,
                "avatars",
                400, // width
                400, // height
                "fill" // crop mode
            );

            _logger.LogInformation("Avatar uploaded successfully for user: {UserId}, URL: {AvatarUrl}", 
                request.UserId, avatarUrl);

            // Update user avatar
            user.Avatar = avatarUrl;
            user.UpdatedAt = DateTime.UtcNow;

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
        catch (Exception ex) when (!(ex is ValidationException || ex is NotFoundException))
        {
            _logger.LogError(ex, "Error uploading avatar for user: {UserId}", request.UserId);
            throw new ConflictException($"Failed to upload avatar: {ex.Message}");
        }
    }
} 