namespace LitePress.WebApi.Endpoints.Posts.Archive;

internal sealed class ArchivePostEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/posts/{id:guid}/archive", HandleAsync)
            .WithName("ArchivePost")
            .WithTags("Posts")
            .WithSummary("Archives a published post.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequireAuthorization();
    }

    private static async Task<IResult> HandleAsync(
        Guid id,
        ICommandMediator commandMediator,
        CancellationToken cancellationToken)
    {
        await commandMediator.SendAsync(new ArchivePostCommand { PostId = new PostId(id) }, cancellationToken);
        return Results.NoContent();
    }
}
