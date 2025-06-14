namespace MyBudgetManagement.Application.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
    Task SendActivateEmailAsync(string to, string fullname, string activateToken);
}