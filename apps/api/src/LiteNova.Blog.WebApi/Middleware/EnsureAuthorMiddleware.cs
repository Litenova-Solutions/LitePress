using LiteNova.Blog.Application.Read.Contracts.Shared;
using LiteNova.Blog.Application.Write.Contracts.Authors.RegisterAuthor;
using LiteNova.Blog.Domain.Authors;
using LiteNova.Blog.WebApi.Extensions;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace LiteNova.Blog.WebApi.Middleware;

internal sealed class EnsureAuthorMiddleware
{
    private readonly RequestDelegate _next;

    public EnsureAuthorMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ICommandMediator commandMediator)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var sub = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? context.User.FindFirst("sub")?.Value;

            if (sub is not null && !context.User.HasClaim("author_id", string.Empty))
            {
                var command = new RegisterAuthorCommand
                {
                    AuthorId = AuthorId.New(),
                    ExternalId = sub,
                    DisplayName = context.User.FindFirst("name")?.Value
                        ?? context.User.FindFirst(ClaimTypes.Name)?.Value
                        ?? sub
                };

                var result = await commandMediator.SendAsync(command, context.RequestAborted);

                var identity = new ClaimsIdentity();
                identity.AddClaim(new Claim("author_id", result.AuthorId.ToString()));

                context.User.AddIdentity(identity);
            }
        }

        await _next(context);
    }
}
