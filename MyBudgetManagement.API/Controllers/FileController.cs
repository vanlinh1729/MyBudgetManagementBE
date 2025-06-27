using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBudgetManagement.Application.Features.Files.Commands.UploadFile;
using MyBudgetManagement.Application.DTOs;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Application.Interfaces;

namespace MyBudgetManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FileController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IFileStorageService _fileStorageService;
    private readonly ILogger<FileController> _logger;

    public FileController(
        IMediator mediator, 
        IFileStorageService fileStorageService,
        ILogger<FileController> logger)
    {
        _mediator = mediator;
        _fileStorageService = fileStorageService;
        _logger = logger;
    }

    /// <summary>
    /// Upload a single file to Cloudinary
    /// </summary>
    /// <param name="file">The file to upload</param>
    /// <param name="folder">Optional folder name (default: uploads)</param>
    /// <param name="width">Optional width for image transformation</param>
    /// <param name="height">Optional height for image transformation</param>
    /// <param name="crop">Optional crop mode (default: fill)</param>
    /// <returns>File upload information</returns>
    [HttpPost("upload")]
    [ProducesResponseType(typeof(ApiResponse<FileUploadDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<string>), 400)]
    public async Task<ActionResult<ApiResponse<FileUploadDto>>> UploadFile(
        IFormFile file,
        [FromQuery] string? folder = "uploads",
        [FromQuery] int? width = null,
        [FromQuery] int? height = null,
        [FromQuery] string crop = "fill")
    {
        try
        {
            var command = new UploadFileCommand
            {
                File = file,
                Folder = folder,
                Width = width,
                Height = height,
                Crop = crop
            };

            var result = await _mediator.Send(command);
            return Ok(new ApiResponse<FileUploadDto> { Data = result, Message = "File uploaded successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file");
            return BadRequest(new ApiResponse<string> { Message = ex.Message });
        }
    }

    /// <summary>
    /// Upload multiple files to Cloudinary
    /// </summary>
    /// <param name="files">The files to upload</param>
    /// <param name="folder">Optional folder name (default: uploads)</param>
    /// <param name="width">Optional width for image transformation</param>
    /// <param name="height">Optional height for image transformation</param>
    /// <param name="crop">Optional crop mode (default: fill)</param>
    /// <returns>List of file upload information</returns>
    [HttpPost("upload-multiple")]
    [ProducesResponseType(typeof(ApiResponse<List<FileUploadDto>>), 200)]
    [ProducesResponseType(typeof(ApiResponse<string>), 400)]
    public async Task<ActionResult<ApiResponse<List<FileUploadDto>>>> UploadMultipleFiles(
        List<IFormFile> files,
        [FromQuery] string? folder = "uploads",
        [FromQuery] int? width = null,
        [FromQuery] int? height = null,
        [FromQuery] string crop = "fill")
    {
        try
        {
            if (files == null || !files.Any())
            {
                return BadRequest(new ApiResponse<string> { Message = "No files provided" });
            }

            var uploadTasks = files.Select(file => _mediator.Send(new UploadFileCommand
            {
                File = file,
                Folder = folder,
                Width = width,
                Height = height,
                Crop = crop
            }));

            var results = await Task.WhenAll(uploadTasks);
            
            return Ok(new ApiResponse<List<FileUploadDto>> 
            { 
                Data = results.ToList(), 
                Message = $"{results.Length} files uploaded successfully" 
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading multiple files");
            return BadRequest(new ApiResponse<string> { Message = ex.Message });
        }
    }

    /// <summary>
    /// Delete a file from Cloudinary by URL
    /// </summary>
    /// <param name="fileUrl">The URL of the file to delete</param>
    /// <returns>Success message</returns>
    [HttpDelete("delete")]
    [ProducesResponseType(typeof(ApiResponse<string>), 200)]
    [ProducesResponseType(typeof(ApiResponse<string>), 400)]
    public async Task<ActionResult<ApiResponse<string>>> DeleteFile([FromQuery] string fileUrl)
    {
        try
        {
            if (string.IsNullOrEmpty(fileUrl))
            {
                return BadRequest(new ApiResponse<string> { Message = "File URL is required" });
            }

            await _fileStorageService.DeleteFileAsync(fileUrl);
            
            return Ok(new ApiResponse<string> 
            { 
                Data = "File deleted successfully", 
                Message = "File deleted successfully" 
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file: {FileUrl}", fileUrl);
            return BadRequest(new ApiResponse<string> { Message = ex.Message });
        }
    }

    /// <summary>
    /// Upload avatar with predefined settings
    /// </summary>
    /// <param name="file">The avatar file to upload</param>
    /// <returns>Avatar upload information</returns>
    [HttpPost("upload-avatar")]
    [ProducesResponseType(typeof(ApiResponse<FileUploadDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<string>), 400)]
    public async Task<ActionResult<ApiResponse<FileUploadDto>>> UploadAvatar(IFormFile file)
    {
        try
        {
            var command = new UploadFileCommand
            {
                File = file,
                Folder = "avatars",
                Width = 400,
                Height = 400,
                Crop = "fill"
            };

            var result = await _mediator.Send(command);
            return Ok(new ApiResponse<FileUploadDto> { Data = result, Message = "Avatar uploaded successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading avatar");
            return BadRequest(new ApiResponse<string> { Message = ex.Message });
        }
    }

    /// <summary>
    /// Get file upload guidelines and limits
    /// </summary>
    /// <returns>Upload guidelines</returns>
    [HttpGet("upload-info")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<object>), 200)]
    public ActionResult<ApiResponse<object>> GetUploadInfo()
    {
        var info = new
        {
            MaxFileSize = "5MB",
            AllowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" },
            SupportedTransformations = new
            {
                CropModes = new[] { "fill", "fit", "crop", "scale", "pad" },
                MaxWidth = 2000,
                MaxHeight = 2000
            },
            DefaultFolders = new[] { "uploads", "avatars", "documents", "images" }
        };

        return Ok(new ApiResponse<object> 
        { 
            Data = info, 
            Message = "Upload information retrieved successfully" 
        });
    }
} 