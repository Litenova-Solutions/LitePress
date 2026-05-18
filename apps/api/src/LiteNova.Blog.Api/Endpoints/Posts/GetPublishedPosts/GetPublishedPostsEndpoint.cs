using LiteBus.Queries.Abstractions;
using LiteNova.Blog.Application.Posts.GetPublishedPosts;

namespace LiteNova.Blog.Api.Endpoints.Posts.GetPublishedPosts;

public static class GetPublishedPostsEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/posts", async (int? page, int? pageSize, IQueryMediator mediator, CancellationToken cancellationToken) =>
            Results.Ok(await mediator.QueryAsync(new GetPublishedPostsQuery(page is > 0 ? page.Value : 1, pageSize is > 0 ? pageSize.Value : 10), cancellationToken)));

        return app;
    }
}
