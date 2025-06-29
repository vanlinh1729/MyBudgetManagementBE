namespace MyBudgetManagement.Application.Features.Auth.Interfaces;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string text, string hash);
}