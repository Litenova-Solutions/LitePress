using LiteBus.Queries.Abstractions;
using LiteNova.Blog.Application.Tags.GetAllTags;

namespace LiteNova.Blog.Api.Endpoints.Tags.GetAllTags;

public static class GetAllTagsEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/tags", async (IQueryMediator mediator, CancellationToken cancellationToken) =>
            Results.Ok(await mediator.QueryAsync(new GetAllTagsQuery(), cancellationToken)));

        return app;
    }
}
