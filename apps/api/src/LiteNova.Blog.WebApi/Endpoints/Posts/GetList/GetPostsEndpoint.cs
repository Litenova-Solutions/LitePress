using LiteNova.Blog.Application.Read.Contracts.Posts.GetAllPosts;
using LiteNova.Blog.Application.Read.Contracts.Posts.GetPublishedPosts;
using LiteNova.Blog.Application.Read.Contracts.Posts.GetPostsByTag;
using LiteNova.Blog.Application.Read.Contracts.Shared;

namespace LiteNova.Blog.WebApi.Endpoints.Posts.GetList;

internal sealed class GetPostsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/posts", HandleAsync)
            .WithName("GetPosts")
            .WithTags("Posts")
            .WithSummary("Returns posts. Admin returns all, public returns published only.")
            .Produces<PagedResult<PostSummaryResult>>(StatusCodes.Status200OK)
            .AllowAnonymous();
    }

    private static async Task<IResult> HandleAsync(
        IQueryMediator queryMediator,
        HttpContext httpContext,
        int page = 1,
        int pageSize = 10,
        string? tag = null,
        CancellationToken cancellationToken = default)
    {
        var pagination = new PaginationParameters { PageNumber = page, PageSize = pageSize };
        bool isAuthenticated = httpContext.User.Identity?.IsAuthenticated == true;

        if (isAuthenticated)
        {
            var query = new GetAllPostsQuery { Pagination = pagination };
            var result = await queryMediator.QueryAsync(query, cancellationToken);
            return Results.Ok(result);
        }
        else if (tag is not null)
        {
            var query = new GetPostsByTagQuery { TagSlug = tag, Pagination = pagination };
            var result = await queryMediator.QueryAsync(query, cancellationToken);
            return Results.Ok(result);
        }
        else
        {
            var query = new GetPublishedPostsQuery { Pagination = pagination };
            var result = await queryMediator.QueryAsync(query, cancellationToken);
            return Results.Ok(result);
        }
    }
}
