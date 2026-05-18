using LiteBus.Commands.Abstractions;
using LiteNova.Blog.Application.Tags.DeleteTag;

namespace LiteNova.Blog.Api.Endpoints.Tags.DeleteTag;

public static class DeleteTagEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/admin/tags/{id:guid}", async (Guid id, ICommandMediator commandMediator, CancellationToken ct) =>
        {
            await commandMediator.SendAsync(new DeleteTagCommand(id), ct);
            return Results.NoContent();
        }).RequireAuthorization();
        return app;
    }
}
