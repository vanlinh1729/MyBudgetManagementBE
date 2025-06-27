using MediatR;
using Microsoft.Extensions.Logging;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.DTOs;
using MyBudgetManagement.Application.Interfaces;

namespace MyBudgetManagement.Application.Features.Files.Commands.UploadFile;

public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, FileUploadDto>
{
    private readonly IFileStorageService _fileStorageService;
    private readonly ILogger<UploadFileCommandHandler> _logger;

    public UploadFileCommandHandler(
        IFileStorageService fileStorageService,
        ILogger<UploadFileCommandHandler> logger)
    {
        _fileStorageService = fileStorageService;
        _logger = logger;
    }

    public async Task<FileUploadDto> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        if (request.File == null || request.File.Length == 0)
        {
            throw new ValidationException("No file provided for upload");
        }

        // Validate file type
        var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };
        if (!allowedTypes.Contains(request.File.ContentType.ToLower()))
        {
            throw new ValidationException("Only image files (JPEG, PNG, GIF, WebP) are allowed");
        }

        // Validate file size (max 5MB)
        const long maxFileSize = 5 * 1024 * 1024; // 5MB
        if (request.File.Length > maxFileSize)
        {
            throw new ValidationException("File size must not exceed 5MB");
        }

        try
        {
            _logger.LogInformation("Starting file upload. FileName: {FileName}, Size: {Size}, ContentType: {ContentType}",
                request.File.FileName, request.File.Length, request.File.ContentType);

            using var stream = request.File.OpenReadStream();
            var fileName = request.File.FileName;
            
            // Generate unique filename to avoid conflicts
            var fileExtension = Path.GetExtension(fileName);
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

            // Use the enhanced upload method with custom parameters
            var fileUrl = await _fileStorageService.UploadFileAsync(
                stream, 
                uniqueFileName, 
                request.Folder ?? "uploads", 
                request.Width, 
                request.Height, 
                request.Crop ?? "fill");

            _logger.LogInformation("File uploaded successfully. URL: {FileUrl}", fileUrl);

            return new FileUploadDto
            {
                Url = fileUrl,
                FileName = fileName,
                Size = request.File.Length,
                ContentType = request.File.ContentType,
                UploadedAt = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file: {FileName}", request.File.FileName);
            throw new ConflictException($"Failed to upload file: {ex.Message}");
        }
    }
} 