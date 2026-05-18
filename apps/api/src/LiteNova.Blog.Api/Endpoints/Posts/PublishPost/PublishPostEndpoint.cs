using LiteBus.Commands.Abstractions;
using LiteNova.Blog.Application.Posts.PublishPost;

namespace LiteNova.Blog.Api.Endpoints.Posts.PublishPost;

public static class PublishPostEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/admin/posts/{id:guid}/publish", async (Guid id, ICommandMediator mediator, CancellationToken cancellationToken) =>
            Results.Ok(await mediator.SendAsync(new PublishPostCommand(id), cancellationToken))).RequireAuthorization();

        return app;
    }
}
