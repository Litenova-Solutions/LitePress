using LiteBus.Queries.Abstractions;
using LiteNova.Blog.Application.Posts.GetAllPosts;

namespace LiteNova.Blog.Api.Endpoints.Posts.GetAllPosts;

public static class GetAllPostsEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/admin/posts", async (int? page, int? pageSize, IQueryMediator mediator, CancellationToken cancellationToken) =>
            Results.Ok(await mediator.QueryAsync(new GetAllPostsQuery(page is > 0 ? page.Value : 1, pageSize is > 0 ? pageSize.Value : 20), cancellationToken)))
            .RequireAuthorization();

        return app;
    }
}
