namespace MyBudgetManagement.Application.DTOs;

public class FileUploadDto
{
    public string Url { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public long Size { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}

public class FileUploadRequestDto
{
    public string? Folder { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public string? Crop { get; set; }
} 