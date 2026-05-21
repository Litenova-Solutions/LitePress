using LiteNova.Blog.WebApi.Extensions;

namespace LiteNova.Blog.WebApi.Endpoints.Posts.Update;

internal sealed record UpdatePostRequest
{
    public required string Title { get; init; }
    public required string Content { get; init; }
    public string? Excerpt { get; init; }
    public string? CoverImageUrl { get; init; }
}

internal sealed record UpdatePostResponse(Guid PostId, string Slug);

internal static class UpdatePostApiMappings
{
    internal static UpdatePostCommand ToCommand(this UpdatePostRequest request, PostId postId)
    {
        return new UpdatePostCommand
        {
            PostId = postId,
            Title = request.Title,
            Content = request.Content,
            Excerpt = request.Excerpt,
            CoverImageUrl = request.CoverImageUrl
        };
    }

    internal static UpdatePostResponse ToResponse(this UpdatePostCommandResult result)
        => new(result.PostId, result.Slug);
}

internal sealed class UpdatePostEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/posts/{id:guid}", HandleAsync)
            .WithName("UpdatePost")
            .WithTags("Posts")
            .WithSummary("Updates a draft post.")
            .Produces<UpdatePostResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequireAuthorization();
    }

    private static async Task<IResult> HandleAsync(
        Guid id,
        UpdatePostRequest request,
        ICommandMediator commandMediator,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(new PostId(id));
        var result = await commandMediator.SendAsync(command, cancellationToken);

        return Results.Ok(result.ToResponse());
    }
}
