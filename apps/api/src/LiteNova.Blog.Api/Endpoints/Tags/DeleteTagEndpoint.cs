using LiteBus.CQRS;
using LiteNova.Blog.Application.Tags.Commands.DeleteTag;

namespace LiteNova.Blog.Api.Endpoints.Tags;

public static class DeleteTagEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/admin/tags/{id:guid}", async (Guid id, IMessageBus bus, CancellationToken ct) =>
        {
            await bus.SendAsync(new DeleteTagCommand(id), ct);
            return Results.NoContent();
        }).RequireAuthorization();
        return app;
    }
}
