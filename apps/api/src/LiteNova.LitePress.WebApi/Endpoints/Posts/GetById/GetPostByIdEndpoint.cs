using LiteNova.LitePress.Application.Read.Contracts.Posts.GetPostById;

namespace LiteNova.LitePress.WebApi.Endpoints.Posts.GetById;

internal sealed class GetPostByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/posts/{id:guid}", HandleAsync)
            .WithName("GetPostById")
            .WithTags("Posts")
            .WithSummary("Returns a post by ID (admin).")
            .Produces<PostDetailResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }

    private static async Task<IResult> HandleAsync(
        Guid id,
        IQueryMediator queryMediator,
        CancellationToken cancellationToken)
    {
        var query = new GetPostByIdQuery { PostId = new PostId(id) };
        var result = await queryMediator.QueryAsync(query, cancellationToken);

        return Results.Ok(result);
    }
}
