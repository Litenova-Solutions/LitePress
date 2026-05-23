using LiteNova.LitePress.WebApi.Extensions;

namespace LiteNova.LitePress.WebApi.Endpoints.Posts.Create;

internal sealed record CreatePostRequest
{
    public required string Title { get; init; }
    public required string Content { get; init; }
    public string? Excerpt { get; init; }
    public string? CoverImageUrl { get; init; }
    public List<Guid> TagIds { get; init; } = [];
}

internal sealed record CreatePostResponse(Guid PostId, string Slug);

internal static class CreatePostApiMappings
{
    internal static CreatePostCommand ToCommand(this CreatePostRequest request, AuthorId authorId)
    {
        return new CreatePostCommand
        {
            PostId = PostId.New(),
            AuthorId = authorId,
            Title = request.Title,
            Content = request.Content,
            Excerpt = request.Excerpt,
            CoverImageUrl = request.CoverImageUrl,
            TagIds = request.TagIds
        };
    }

    internal static CreatePostResponse ToResponse(this CreatePostCommandResult result)
        => new(result.PostId, result.Slug);
}

internal sealed class CreatePostEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/posts", HandleAsync)
            .WithName("CreatePost")
            .WithTags("Posts")
            .WithSummary("Creates a new draft post.")
            .Produces<CreatePostResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .RequireAuthorization();
    }

    private static async Task<IResult> HandleAsync(
        CreatePostRequest request,
        ICommandMediator commandMediator,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var authorId = httpContext.User.GetAuthorId();
        var command = request.ToCommand(authorId);
        var result = await commandMediator.SendAsync(command, cancellationToken);
        var response = result.ToResponse();

        return Results.Created($"/api/posts/{response.PostId}", response);
    }
}
