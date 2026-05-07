using LiteBus.CQRS;
using LiteNova.Blog.Application.Posts.Queries.GetPublishedPosts;

namespace LiteNova.Blog.Api.Endpoints.Posts;

public static class GetPublishedPostsEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/posts", async (int page, int pageSize, IMessageBus bus, CancellationToken ct) =>
            Results.Ok(await bus.QueryAsync(new GetPublishedPostsQuery(page <= 0 ? 1 : page, pageSize <= 0 ? 10 : pageSize), ct)));
        return app;
    }
}
