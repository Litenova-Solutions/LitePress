using LiteBus.CQRS;
using LiteNova.Blog.Api.Models.Requests;
using LiteNova.Blog.Application.Tags.Commands.CreateTag;
using Mapster;

namespace LiteNova.Blog.Api.Endpoints.Tags;

public static class CreateTagEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/admin/tags", async (CreateTagRequest request, IMessageBus bus, CancellationToken ct) =>
            Results.Ok(await bus.SendAsync(request.Adapt<CreateTagCommand>(), ct))).RequireAuthorization();
        return app;
    }
}
