using LiteNova.Blog.Application.Write.Contracts.Shared.Exceptions;
using LiteNova.Blog.Domain.Shared.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace LiteNova.Blog.WebApi.Middleware;

internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);

        int statusCode;
        string title;
        string detail;
        object? extensions;

        switch (exception)
        {
            case CommandValidationException validationEx:
                statusCode = StatusCodes.Status400BadRequest;
                title = "Command validation failure";
                detail = "One or more validation errors occurred.";
                extensions = new Dictionary<string, object>
                {
                    ["errors"] = new Dictionary<string, string[]>
                    {
                        ["error"] = [validationEx.Message]
                    }
                };
                break;
            case AggregateNotFoundException notFoundEx:
                statusCode = StatusCodes.Status404NotFound;
                title = "Resource not found";
                detail = notFoundEx.Message;
                extensions = null;
                break;
            case DomainException domainEx:
                statusCode = StatusCodes.Status409Conflict;
                title = "Domain rule violation";
                detail = domainEx.Message;
                extensions = null;
                break;
            default:
                statusCode = StatusCodes.Status500InternalServerError;
                title = "An unexpected error occurred";
                detail = "An internal server error has occurred.";
                extensions = null;
                break;
        }

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        if (extensions is not null)
        {
            problem.Extensions["extensions"] = extensions;
        }

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }
}
