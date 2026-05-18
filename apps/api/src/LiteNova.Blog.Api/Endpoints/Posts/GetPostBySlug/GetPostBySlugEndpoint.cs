using LiteBus.Queries.Abstractions;
using LiteNova.Blog.Application.Posts.GetPostBySlug;

namespace LiteNova.Blog.Api.Endpoints.Posts.GetPostBySlug;

public static class GetPostBySlugEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/posts/{slug}", async (string slug, IQueryMediator queryMediator, CancellationToken ct) =>
            Results.Ok(await queryMediator.QueryAsync(new GetPostBySlugQuery(slug), ct)));
        return app;
    }
}
