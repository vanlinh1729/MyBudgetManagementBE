namespace MyBudgetManagement.Application.Features.Auth.Dtos;

public class RegisterRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
}