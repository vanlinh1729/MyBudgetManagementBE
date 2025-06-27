using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Interfaces;

namespace MyBudgetManagement.Infrastructure.FileStorage;

public class CloudinaryService : IFileStorageService
{
    private readonly Cloudinary _cloudinary;
    private readonly ILogger<CloudinaryService> _logger;

    public CloudinaryService(IConfiguration configuration, ILogger<CloudinaryService> logger)
    {
        _logger = logger;

        try 
        {
            var cloudinarySection = configuration.GetSection("Cloudinary");
            _logger.LogInformation("Cloudinary Section exists: {exists}", cloudinarySection.Exists());
            
            var cloudName = cloudinarySection["CloudName"];
            var apiKey = cloudinarySection["ApiKey"];
            var apiSecret = cloudinarySection["ApiSecret"];

            _logger.LogInformation("Cloudinary Config - CloudName: {cloudName}, ApiKey: {apiKeyExists}, ApiSecret: {apiSecretExists}", 
                cloudName,
                !string.IsNullOrEmpty(apiKey),
                !string.IsNullOrEmpty(apiSecret));

            if (string.IsNullOrEmpty(cloudName) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
            {
                throw new ConflictException("Cloudinary configuration is missing or invalid");
            }

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initializing Cloudinary service");
            throw new ConflictException($"Failed to initialize Cloudinary: {ex.Message}");
        }
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
        return await UploadFileAsync(fileStream, fileName, "uploads", 800, 600, "fill");
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string folder = "uploads", int? width = null, int? height = null, string crop = "fill")
    {
        if (fileStream == null)
        {
            throw new ConflictException("File stream is null");
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new ConflictException("File name is empty");
        }

        try
        {
            _logger.LogInformation("Attempting to upload file: {fileName} to folder: {folder}", fileName, folder);

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileStream),
                Folder = folder,
                UseFilename = false,
                UniqueFilename = true,
                Overwrite = false
            };

            // Add transformation if width and height are specified
            if (width.HasValue && height.HasValue)
            {
                uploadParams.Transformation = new Transformation()
                    .Width(width.Value)
                    .Height(height.Value)
                    .Crop(crop)
                    .Quality("auto")
                    .FetchFormat("auto");
            }
            else if (width.HasValue || height.HasValue)
            {
                var transformation = new Transformation().Quality("auto").FetchFormat("auto");
                
                if (width.HasValue)
                    transformation = transformation.Width(width.Value);
                if (height.HasValue)
                    transformation = transformation.Height(height.Value);
                    
                uploadParams.Transformation = transformation.Crop(crop);
            }

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            
            if (uploadResult.Error != null)
            {
                _logger.LogError("Cloudinary upload error: {error}", uploadResult.Error.Message);
                throw new ConflictException($"Cloudinary upload failed: {uploadResult.Error.Message}");
            }

            _logger.LogInformation("File uploaded successfully: {url}", uploadResult.SecureUrl);
            return uploadResult.SecureUrl.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file {fileName}", fileName);
            throw new ConflictException($"File upload failed: {ex.Message}");
        }
    }

    public async Task DeleteFileAsync(string fileUrl)
    {
        if (string.IsNullOrEmpty(fileUrl))
        {
            _logger.LogInformation("No file URL provided for deletion");
            return;
        }

        try
        {
            _logger.LogInformation("Attempting to delete file: {fileUrl}", fileUrl);

            // Extract public_id from URL
            var publicId = ExtractPublicIdFromUrl(fileUrl);
            
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            if (result.Error != null)
            {
                _logger.LogError("Cloudinary delete error: {error}", result.Error.Message);
                throw new ConflictException($"Cloudinary delete failed: {result.Error.Message}");
            }

            _logger.LogInformation("File deleted successfully. PublicId: {publicId}", publicId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file {fileUrl}", fileUrl);
            throw new ConflictException($"File deletion failed: {ex.Message}");
        }
    }

    private string ExtractPublicIdFromUrl(string url)
    {
        try
        {
            var uri = new Uri(url);
            var path = uri.AbsolutePath;
            
            // Remove the version number if present (v1234567890)
            var segments = path.Split('/');
            var relevantSegments = segments.Skip(segments.Length >= 3 && segments[segments.Length - 3].StartsWith("v") ? 1 : 0);
            
            var pathWithoutVersion = string.Join("/", relevantSegments);
            
            // Remove file extension
            var lastDotIndex = pathWithoutVersion.LastIndexOf('.');
            if (lastDotIndex > 0)
            {
                pathWithoutVersion = pathWithoutVersion.Substring(0, lastDotIndex);
            }
            
            // Remove leading slash
            return pathWithoutVersion.TrimStart('/');
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error extracting public ID from URL: {url}", url);
            // Fallback method
            var segments = url.Split('/');
            var fileName = segments[segments.Length - 1];
            return $"uploads/{Path.GetFileNameWithoutExtension(fileName)}";
        }
    }
}