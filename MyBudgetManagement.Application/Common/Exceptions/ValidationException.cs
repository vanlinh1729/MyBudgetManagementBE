namespace MyBudgetManagement.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public List<string> Errors { get; }

    public ValidationException(string message) : base(message)
    {
        Errors = new List<string> { message };
    }

    public ValidationException(IEnumerable<string> errors) : base("Validation failed")
    {
        Errors = errors.ToList();
    }
} 