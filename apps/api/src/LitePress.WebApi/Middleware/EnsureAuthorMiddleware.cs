using LitePress.Application.Read.Contracts.Shared;
using LitePress.Application.Write.Contracts.Authors.RegisterAuthor;
using LitePress.Domain.Authors;
using LitePress.WebApi.Extensions;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace LitePress.WebApi.Middleware;

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

            if (sub is not null && context.User.FindFirst("author_id") is null)
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
