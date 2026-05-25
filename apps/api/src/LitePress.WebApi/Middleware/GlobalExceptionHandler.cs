using LitePress.Application.Read.Contracts.Shared.Exceptions;
using LitePress.Application.Write.Contracts.Shared.Exceptions;
using LitePress.Domain.Shared.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LitePress.WebApi.Middleware;

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
        var (statusCode, problem) = exception switch
        {
            CommandValidationException validationEx =>
                (StatusCodes.Status400BadRequest, BuildValidationProblem(
                    validationEx.Message,
                    httpContext)),

            QueryValidationException queryEx =>
                (StatusCodes.Status400BadRequest, BuildValidationProblem(
                    queryEx.Message,
                    httpContext)),

            AggregateNotFoundException notFoundEx =>
                (StatusCodes.Status404NotFound, new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Resource not found.",
                    Detail = notFoundEx.Message,
                    Instance = httpContext.Request.Path
                }),

            DomainException domainEx =>
                (StatusCodes.Status409Conflict, new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "Conflict",
                    Detail = domainEx.Message,
                    Instance = httpContext.Request.Path
                }),

            DbUpdateConcurrencyException =>
                (StatusCodes.Status409Conflict, new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "Conflict",
                    Detail = "The resource was modified by another actor. Retrieve the latest version and retry.",
                    Instance = httpContext.Request.Path
                }),

            _ =>
                (StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Internal server error.",
                    Detail = "An unexpected error occurred. Please contact support.",
                    Instance = httpContext.Request.Path
                })
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception");
        }
        else if (exception is DomainException or AggregateNotFoundException)
        {
            _logger.LogWarning(
                exception,
                "Expected failure ({StatusCode}): {Message}",
                statusCode,
                exception.Message);
        }

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/problem+json";
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }

    private static object BuildValidationProblem(string message, HttpContext httpContext)
    {
        return new
        {
            type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            title = "Validation failed.",
            status = StatusCodes.Status400BadRequest,
            detail = "One or more fields failed validation.",
            instance = httpContext.Request.Path.Value,
            invalidParams = new[]
            {
                new { name = "error", reason = message }
            }
        };
    }
}
