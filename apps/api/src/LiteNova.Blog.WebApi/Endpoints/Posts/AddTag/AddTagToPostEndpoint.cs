namespace LiteNova.Blog.WebApi.Endpoints.Posts.AddTag;

internal sealed record AddTagToPostRequest(Guid TagId);

internal sealed record AddTagToPostResponse(Guid PostId, Guid TagId);

internal static class AddTagToPostApiMappings
{
    internal static AddTagToPostCommand ToCommand(this AddTagToPostRequest request, PostId postId)
        => new() { PostId = postId, TagId = new TagId(request.TagId) };

    internal static AddTagToPostResponse ToResponse(this AddTagToPostCommandResult result)
        => new(result.PostId, result.TagId);
}

internal sealed class AddTagToPostEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/posts/{id:guid}/tags", HandleAsync)
            .WithName("AddTagToPost")
            .WithTags("Posts")
            .WithSummary("Adds a tag to a post.")
            .Produces<AddTagToPostResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization();
    }

    private static async Task<IResult> HandleAsync(
        Guid id,
        AddTagToPostRequest request,
        ICommandMediator commandMediator,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(new PostId(id));
        var result = await commandMediator.SendAsync(command, cancellationToken);

        return Results.Ok(result.ToResponse());
    }
}
