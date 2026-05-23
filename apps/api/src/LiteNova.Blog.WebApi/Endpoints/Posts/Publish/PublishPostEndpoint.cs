namespace LiteNova.Blog.WebApi.Endpoints.Posts.Publish;

internal sealed class PublishPostEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/posts/{id:guid}/publish", HandleAsync)
            .WithName("PublishPost")
            .WithTags("Posts")
            .WithSummary("Publishes a draft post.")
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
        await commandMediator.SendAsync(
            new PublishPostCommand { PostId = new PostId(id) },
            cancellationToken);
        return Results.NoContent();
    }
}
