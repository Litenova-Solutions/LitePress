using LiteBus.CQRS;
using LiteNova.Blog.Api.Models.Requests;
using LiteNova.Blog.Application.Posts.Commands.CreatePost;
using Mapster;

namespace LiteNova.Blog.Api.Endpoints.Posts;

public static class CreatePostEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/admin/posts", async (CreatePostRequest request, IMessageBus bus, CancellationToken ct) =>
        {
            var result = await bus.SendAsync(request.Adapt<CreatePostCommand>(), ct);
            return Results.Created($"/api/posts/{result.Id}", result);
        }).RequireAuthorization();
        return app;
    }
}
