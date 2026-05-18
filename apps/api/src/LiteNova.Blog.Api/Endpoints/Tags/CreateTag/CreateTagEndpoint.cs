using LiteBus.Commands.Abstractions;
using LiteNova.Blog.Api.Models.Requests;
using LiteNova.Blog.Application.Tags.CreateTag;
using Mapster;

namespace LiteNova.Blog.Api.Endpoints.Tags.CreateTag;

public static class CreateTagEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/admin/tags", async (CreateTagRequest request, ICommandMediator commandMediator, CancellationToken ct) =>
            Results.Ok(await commandMediator.SendAsync(request.Adapt<CreateTagCommand>(), ct))).RequireAuthorization();
        return app;
    }
}
