using LiteBus.Queries.Abstractions;
using LiteNova.Blog.Application.Posts.GetAllPosts;

namespace LiteNova.Blog.Api.Endpoints.Posts.GetAllPosts;

public static class GetAllPostsEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/admin/posts", async (int page, int pageSize, IQueryMediator queryMediator, CancellationToken ct) =>
            Results.Ok(await queryMediator.QueryAsync(new GetAllPostsQuery(page <= 0 ? 1 : page, pageSize <= 0 ? 20 : pageSize), ct))).RequireAuthorization();
        return app;
    }
}
