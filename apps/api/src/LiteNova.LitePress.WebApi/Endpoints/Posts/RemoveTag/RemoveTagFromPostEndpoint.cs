namespace LiteNova.LitePress.WebApi.Endpoints.Posts.RemoveTag;

internal sealed record RemoveTagFromPostResponse(Guid PostId, Guid TagId);

internal static class RemoveTagFromPostApiMappings
{
    internal static RemoveTagFromPostCommand ToCommand(PostId postId, TagId tagId)
        => new() { PostId = postId, TagId = tagId };

    internal static RemoveTagFromPostResponse ToResponse(this RemoveTagFromPostCommandResult result)
        => new(result.PostId, result.TagId);
}

internal sealed class RemoveTagFromPostEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/posts/{id:guid}/tags/{tagId:guid}", HandleAsync)
            .WithName("RemoveTagFromPost")
            .WithTags("Posts")
            .WithSummary("Removes a tag from a post.")
            .Produces<RemoveTagFromPostResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequireAuthorization();
    }

    private static async Task<IResult> HandleAsync(
        Guid id,
        Guid tagId,
        ICommandMediator commandMediator,
        CancellationToken cancellationToken)
    {
        var command = RemoveTagFromPostApiMappings.ToCommand(new PostId(id), new TagId(tagId));
        var result = await commandMediator.SendAsync(command, cancellationToken);

        return Results.Ok(result.ToResponse());
    }
}
