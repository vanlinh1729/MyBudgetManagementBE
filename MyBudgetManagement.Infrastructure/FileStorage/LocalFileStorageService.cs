using Microsoft.AspNetCore.Hosting;
using MyBudgetManagement.Application.Interfaces;

namespace MyBudgetManagement.Infrastructure.FileStorage;

public class LocalFileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _environment;
    private readonly string _uploadDirectory;

    public LocalFileStorageService(IWebHostEnvironment environment)
    {
        _environment = environment;
        _uploadDirectory = Path.Combine(_environment.WebRootPath, "uploads");
        if (!Directory.Exists(_uploadDirectory))
        {
            Directory.CreateDirectory(_uploadDirectory);
        }
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
        return await UploadFileAsync(fileStream, fileName, "uploads");
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string folder = "uploads", int? width = null, int? height = null, string crop = "fill")
    {
        // Create folder path
        var folderPath = Path.Combine(_uploadDirectory, folder);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var filePath = Path.Combine(folderPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await fileStream.CopyToAsync(stream);
        }

        // Return relative path for local storage
        return Path.Combine(folder, fileName).Replace('\\', '/');
    }

    public Task DeleteFileAsync(string fileName)
    {
        var filePath = Path.Combine(_uploadDirectory, fileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        return Task.CompletedTask;
    }
}