using LiteNova.LitePress.Application.Read.Contracts.Posts.GetPostById;
using LiteNova.LitePress.Application.Read.Contracts.Posts.GetPostBySlug;

namespace LiteNova.LitePress.WebApi.Endpoints.Posts.GetBySlug;

internal sealed class GetPostBySlugEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/posts/{slug}", HandleAsync)
            .WithName("GetPostBySlug")
            .WithTags("Posts")
            .WithSummary("Returns a published post by slug (public).")
            .Produces<PostDetailResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .AllowAnonymous();
    }

    private static async Task<IResult> HandleAsync(
        string slug,
        IQueryMediator queryMediator,
        CancellationToken cancellationToken)
    {
        var query = new GetPostBySlugQuery { Slug = slug };
        var result = await queryMediator.QueryAsync(query, cancellationToken);

        return Results.Ok(result);
    }
}
