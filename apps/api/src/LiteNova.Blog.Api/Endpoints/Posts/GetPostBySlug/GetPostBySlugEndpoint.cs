using LiteBus.Queries.Abstractions;
using LiteNova.Blog.Application.Posts.GetPostBySlug;

namespace LiteNova.Blog.Api.Endpoints.Posts.GetPostBySlug;

public static class GetPostBySlugEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/posts/{slug}", async (string slug, IQueryMediator mediator, CancellationToken cancellationToken) =>
            Results.Ok(await mediator.QueryAsync(new GetPostBySlugQuery(slug), cancellationToken)));

        return app;
    }
}
