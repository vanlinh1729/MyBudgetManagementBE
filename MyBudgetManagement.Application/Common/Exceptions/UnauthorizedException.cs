using System.Net;

namespace MyBudgetManagement.Application.Common.Exceptions;

public class UnauthorizedException : Exception
{
    public HttpStatusCode StatusCode { get; } = HttpStatusCode.Unauthorized;
    public string ErrorCode { get; }

    public UnauthorizedException(string message, string errorCode = "unauthorized") 
        : base(message)
    {
        ErrorCode = errorCode;
    }
}