using LiteBus.Commands.Abstractions;
using LiteNova.Blog.Application.Posts.DeletePost;

namespace LiteNova.Blog.Api.Endpoints.Posts.DeletePost;

public static class DeletePostEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/admin/posts/{id:guid}", async (Guid id, ICommandMediator commandMediator, CancellationToken ct) =>
        {
            await commandMediator.SendAsync(new DeletePostCommand(id), ct);
            return Results.NoContent();
        }).RequireAuthorization();
        return app;
    }
}
