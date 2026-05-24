namespace LitePress.WebApi.Endpoints.Tags.Delete;

internal sealed class DeleteTagEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/tags/{id:guid}", HandleAsync)
            .WithName("DeleteTag")
            .WithTags("Tags")
            .WithSummary("Deletes a tag.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }

    private static async Task<IResult> HandleAsync(
        Guid id,
        ICommandMediator commandMediator,
        CancellationToken cancellationToken)
    {
        await commandMediator.SendAsync(new DeleteTagCommand { TagId = new TagId(id) }, cancellationToken);
        return Results.NoContent();
    }
}
