using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Application.Features.Users.Dtos;

public class UserProfileDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? PhoneNumber { get; set; }
    public AccountStatus Status { get; set; }
    public Currencies Currency { get; set; }
    public DateTime? LastChangePassword { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    
    // Navigation properties
    public decimal CurrentBalance { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
}

public class UpdateUserProfileDto
{
    public string FullName { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? PhoneNumber { get; set; }
    public Currencies Currency { get; set; }
}

public class ChangePasswordDto
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
} 