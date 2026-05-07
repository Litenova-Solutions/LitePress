using LiteBus.CQRS;
using LiteNova.Blog.Application.Posts.Queries.GetPostBySlug;

namespace LiteNova.Blog.Api.Endpoints.Posts;

public static class GetPostBySlugEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/posts/{slug}", async (string slug, IMessageBus bus, CancellationToken ct) =>
            Results.Ok(await bus.QueryAsync(new GetPostBySlugQuery(slug), ct)));
        return app;
    }
}
