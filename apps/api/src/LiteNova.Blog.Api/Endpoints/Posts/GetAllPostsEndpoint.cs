using LiteBus.CQRS;
using LiteNova.Blog.Application.Posts.Queries.GetAllPosts;

namespace LiteNova.Blog.Api.Endpoints.Posts;

public static class GetAllPostsEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/admin/posts", async (int page, int pageSize, IMessageBus bus, CancellationToken ct) =>
            Results.Ok(await bus.QueryAsync(new GetAllPostsQuery(page <= 0 ? 1 : page, pageSize <= 0 ? 20 : pageSize), ct))).RequireAuthorization();
        return app;
    }
}
