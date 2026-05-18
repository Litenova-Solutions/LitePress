using LiteNova.Blog.Application.Common.Exceptions;
using LiteNova.Blog.Domain.Posts.Exceptions;

namespace LiteNova.Blog.Api.Middleware;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex switch
            {
                PostNotFoundException or TagNotFoundException => StatusCodes.Status404NotFound,
                PostAlreadyPublishedException or PostAlreadyScheduledException => StatusCodes.Status409Conflict,
                InvalidPostSlugException or DuplicateSlugException => StatusCodes.Status422UnprocessableEntity,
                ValidationException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            if (ex is ValidationException vex)
            {
                await context.Response.WriteAsJsonAsync(new { error = vex.Message, details = vex.Errors });
                return;
            }

            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
    }
}
