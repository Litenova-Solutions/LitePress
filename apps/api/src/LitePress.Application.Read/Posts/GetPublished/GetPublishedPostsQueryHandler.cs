using LitePress.Application.Read.Contracts.Posts.GetAllPosts;
using LitePress.Application.Read.Contracts.Posts.GetPublishedPosts;

namespace LitePress.Application.Read.Posts.GetPublished;

internal sealed class GetPublishedPostsQueryHandler : IQueryHandler<GetPublishedPostsQuery, PagedResult<PostSummaryResult>>
{
    private readonly IReadDatabase _db;
    public GetPublishedPostsQueryHandler(IReadDatabase db) { _db = db; }

    public Task<PagedResult<PostSummaryResult>> HandleAsync(GetPublishedPostsQuery query, CancellationToken cancellationToken) =>
        _db.QueryAsync(async (ctx, ct) =>
        {
            var pageSize = Math.Min(query.Pagination.PageSize, PaginationParameters.MaxPageSize);
            var pageNumber = query.Pagination.PageNumber;
            var start = (pageNumber - 1) * pageSize;

            var allPosts = await ctx.ToListAsync(ctx.Posts, ct);
            var publishedPosts = allPosts
                .Where(post => post.State is PublishedPostState)
                .OrderByDescending(post => ((PublishedPostState)post.State).PublishedAt)
                .ToList();

            var totalCount = query.Pagination.SkipTotalCount ? 0 : publishedPosts.Count;

            var posts = publishedPosts
                .Skip(start)
                .Take(pageSize)
                .ToList();

            var authorIds = posts.Select(post => post.AuthorId.Value).Distinct().ToList();
            var authors = await PostReadSupport.LoadAuthorNamesAsync(ctx, authorIds, ct);
            var tagLookup = await PostReadSupport.LoadTagSummariesAsync(
                ctx,
                posts.SelectMany(post => post.Tags.Select(tag => tag.TagId.Value)).Distinct().ToList(),
                ct);

            var items = posts.Select(post => new PostSummaryResult(
                post.Id.Value,
                post.Title.Value,
                post.Slug.Value,
                post.Excerpt?.Value,
                post.CoverImageUrl?.Value,
                authors.GetValueOrDefault(post.AuthorId.Value, string.Empty),
                "Published",
                post.CreatedAt,
                PostStateQuery.GetPublishedAt(post.State),
                post.Tags.Select(tag => tagLookup.GetValueOrDefault(
                    tag.TagId.Value,
                    new TagSummaryResult(tag.TagId.Value, string.Empty, string.Empty))).ToList()
            )).ToList();

            return new PagedResult<PostSummaryResult>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }, cancellationToken);
}
