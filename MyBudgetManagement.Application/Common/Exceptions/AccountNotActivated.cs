using System.Net;

namespace MyBudgetManagement.Application.Common.Exceptions;

public class AccountNotActivated: Exception
{
    public HttpStatusCode StatusCode { get; } = HttpStatusCode.Unauthorized;
    public string ErrorCode { get; }

    public AccountNotActivated(string message, string errorCode = "Account not activated.") 
        : base(message)
    {
        ErrorCode = errorCode;
    }
}