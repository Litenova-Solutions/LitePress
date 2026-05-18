using LiteBus.Commands.Abstractions;
using LiteNova.Blog.Api.Models.Requests;
using LiteNova.Blog.Application.Tags.CreateTag;
using Mapster;

namespace LiteNova.Blog.Api.Endpoints.Tags.CreateTag;

public static class CreateTagEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/admin/tags", async (CreateTagRequest request, ICommandMediator mediator, CancellationToken cancellationToken) =>
            Results.Ok(await mediator.SendAsync(request.Adapt<CreateTagCommand>(), cancellationToken))).RequireAuthorization();

        return app;
    }
}
