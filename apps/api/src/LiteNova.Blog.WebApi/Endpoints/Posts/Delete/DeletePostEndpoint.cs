namespace LiteNova.Blog.WebApi.Endpoints.Posts.Delete;

internal sealed class DeletePostEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/posts/{id:guid}", HandleAsync)
            .WithName("DeletePost")
            .WithTags("Posts")
            .WithSummary("Deletes a draft or archived post.")
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
        await commandMediator.SendAsync(new DeletePostCommand { PostId = new PostId(id) }, cancellationToken);
        return Results.NoContent();
    }
}
