using LitePress.Application.Read.Contracts.Tags.GetAllTags;
using LitePress.Application.Read.Contracts.Shared;

namespace LitePress.WebApi.Endpoints.Tags.GetAll;

internal sealed class GetAllTagsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/tags", HandleAsync)
            .WithName("GetAllTags")
            .WithTags("Tags")
            .WithSummary("Returns all tags with post counts.")
            .Produces<IReadOnlyList<TagResult>>(StatusCodes.Status200OK)
            .AllowAnonymous();
    }

    private static async Task<IResult> HandleAsync(
        IQueryMediator queryMediator,
        CancellationToken cancellationToken)
    {
        var result = await queryMediator.QueryAsync(new GetAllTagsQuery(), cancellationToken);
        return Results.Ok(result);
    }
}
