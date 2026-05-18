using LiteBus.Queries.Abstractions;
using LiteNova.Blog.Application.Posts.GetPublishedPosts;

namespace LiteNova.Blog.Api.Endpoints.Posts.GetPublishedPosts;

public static class GetPublishedPostsEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/posts", async (int page, int pageSize, IQueryMediator queryMediator, CancellationToken ct) =>
            Results.Ok(await queryMediator.QueryAsync(new GetPublishedPostsQuery(page <= 0 ? 1 : page, pageSize <= 0 ? 10 : pageSize), ct)));
        return app;
    }
}
