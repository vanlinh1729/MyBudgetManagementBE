using MediatR;
using Microsoft.AspNetCore.Http;
using MyBudgetManagement.Application.DTOs;

namespace MyBudgetManagement.Application.Features.Files.Commands.UploadFile;

public class UploadFileCommand : IRequest<FileUploadDto>
{
    public IFormFile File { get; set; } = null!;
    public string? Folder { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public string? Crop { get; set; } = "fill";
} 