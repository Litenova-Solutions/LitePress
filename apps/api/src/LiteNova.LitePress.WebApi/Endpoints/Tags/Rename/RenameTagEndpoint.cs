namespace LiteNova.LitePress.WebApi.Endpoints.Tags.Rename;

internal sealed record RenameTagRequest(string Name);

internal sealed class RenameTagEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/tags/{id:guid}", HandleAsync)
            .WithName("RenameTag")
            .WithTags("Tags")
            .WithSummary("Renames an existing tag.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }

    private static async Task<IResult> HandleAsync(
        Guid id,
        RenameTagRequest request,
        ICommandMediator commandMediator,
        CancellationToken cancellationToken)
    {
        var command = new RenameTagCommand { TagId = new TagId(id), NewName = request.Name };
        await commandMediator.SendAsync(command, cancellationToken);

        return Results.NoContent();
    }
}
