using LiteBus.CQRS;
using LiteNova.Blog.Application.Posts.Commands.PublishPost;

namespace LiteNova.Blog.Api.Endpoints.Posts;

public static class PublishPostEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/admin/posts/{id:guid}/publish", async (Guid id, IMessageBus bus, CancellationToken ct) =>
            Results.Ok(await bus.SendAsync(new PublishPostCommand(id), ct))).RequireAuthorization();
        return app;
    }
}
