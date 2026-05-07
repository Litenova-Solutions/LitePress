using LiteBus.CQRS;
using LiteNova.Blog.Application.Posts.Commands.DeletePost;

namespace LiteNova.Blog.Api.Endpoints.Posts;

public static class DeletePostEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/admin/posts/{id:guid}", async (Guid id, IMessageBus bus, CancellationToken ct) =>
        {
            await bus.SendAsync(new DeletePostCommand(id), ct);
            return Results.NoContent();
        }).RequireAuthorization();
        return app;
    }
}
