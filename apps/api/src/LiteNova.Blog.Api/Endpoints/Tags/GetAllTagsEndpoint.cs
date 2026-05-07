using LiteBus.CQRS;
using LiteNova.Blog.Application.Tags.Queries.GetAllTags;

namespace LiteNova.Blog.Api.Endpoints.Tags;

public static class GetAllTagsEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/tags", async (IMessageBus bus, CancellationToken ct) =>
            Results.Ok(await bus.QueryAsync(new GetAllTagsQuery(), ct)));
        return app;
    }
}
