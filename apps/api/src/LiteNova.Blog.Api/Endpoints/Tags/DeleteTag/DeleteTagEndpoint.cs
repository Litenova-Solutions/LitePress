using LiteBus.Commands.Abstractions;
using LiteNova.Blog.Application.Tags.DeleteTag;

namespace LiteNova.Blog.Api.Endpoints.Tags.DeleteTag;

public static class DeleteTagEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/admin/tags/{id:guid}", async (Guid id, ICommandMediator mediator, CancellationToken cancellationToken) =>
        {
            await mediator.SendAsync(new DeleteTagCommand(id), cancellationToken);
            return Results.NoContent();
        }).RequireAuthorization();

        return app;
    }
}
