namespace MyBudgetManagement.Application.Interfaces;

public interface IFileStorageService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName);
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string folder = "uploads", int? width = null, int? height = null, string crop = "fill");
    Task DeleteFileAsync(string fileName);
}