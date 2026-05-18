using LiteBus.Queries.Abstractions;
using LiteNova.Blog.Application.Tags.GetAllTags;

namespace LiteNova.Blog.Api.Endpoints.Tags.GetAllTags;

public static class GetAllTagsEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/tags", async (IQueryMediator queryMediator, CancellationToken ct) =>
            Results.Ok(await queryMediator.QueryAsync(new GetAllTagsQuery(), ct)));
        return app;
    }
}
