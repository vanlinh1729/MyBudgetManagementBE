using System.Net;
using System.Text.Json;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Wrappers;

namespace MyBudgetManagement.API.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An unexpected error occurred: {Message}", exception.Message);
            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var response = new ApiResponse<object>
        {
            Succeeded = false,
            Data = null
        };

        switch (exception)
        {
            case UnauthorizedException ex:
                response.Message = ex.Message;
                response.Errors = new List<string> { ex.Message };
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                break;

            case AccountNotActivated ex:
                response.Message = ex.Message;
                response.Errors = new List<string> { ex.ErrorCode };
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                break;

            case NotFoundException ex:
                response.Message = ex.Message;
                response.Errors = new List<string> { ex.Message };
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                break;

            case ConflictException ex:
                response.Message = ex.Message;
                response.Errors = new List<string> { ex.Message };
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                break;

            case ValidationException ex:
                response.Message = "Validation failed";
                response.Errors = ex.Errors?.ToList() ?? new List<string> { ex.Message };
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;

            case ArgumentException ex:
                response.Message = "Invalid request";
                response.Errors = new List<string> { ex.Message };
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;

            default:
                response.Message = "An error occurred while processing your request";
                response.Errors = new List<string> { "Internal server error" };
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}

public class ValidationException : Exception
{
    public IEnumerable<string> Errors { get; }

    public ValidationException(string message) : base(message)
    {
        Errors = new List<string> { message };
    }

    public ValidationException(IEnumerable<string> errors) : base("Validation failed")
    {
        Errors = errors;
    }
} 