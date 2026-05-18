using LiteBus.Commands.Abstractions;
using LiteNova.Blog.Api.Models.Requests;
using LiteNova.Blog.Application.Posts.CreatePost;
using Mapster;

namespace LiteNova.Blog.Api.Endpoints.Posts.CreatePost;

public static class CreatePostEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/admin/posts", async (CreatePostRequest request, ICommandMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.SendAsync(request.Adapt<CreatePostCommand>(), cancellationToken);
            return Results.Created($"/api/admin/posts/{result.Id}", result);
        }).RequireAuthorization();

        return app;
    }
}
