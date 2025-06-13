namespace MyBudgetManagement.Application.Features.Auth.Dtos;

public class AuthResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}