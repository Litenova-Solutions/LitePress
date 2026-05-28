using LitePress.Application.Read.Contracts.Posts.GetAllPosts;

namespace LitePress.Application.Read.Posts.GetAll;

internal sealed class GetAllPostsQueryHandler : IQueryHandler<GetAllPostsQuery, PagedResult<PostSummaryResult>>
{
    private readonly IReadDatabase _db;
    public GetAllPostsQueryHandler(IReadDatabase db) { _db = db; }

    public Task<PagedResult<PostSummaryResult>> HandleAsync(GetAllPostsQuery query, CancellationToken cancellationToken) =>
        _db.QueryAsync(async (ctx, ct) =>
        {
            var pageSize = Math.Min(query.Pagination.PageSize, PaginationParameters.MaxPageSize);
            var pageNumber = query.Pagination.PageNumber;
            var start = (pageNumber - 1) * pageSize;

            var totalCount = 0;
            if (!query.Pagination.SkipTotalCount)
            {
                totalCount = await ctx.CountAsync(ctx.Posts, ct);
            }

            var posts = await ctx.ToListAsync(
                ctx.Posts
                    .OrderByDescending(post => post.CreatedAt)
                    .Skip(start)
                    .Take(pageSize),
                ct);

            if (posts.Count == 0)
            {
                return new PagedResult<PostSummaryResult>
                {
                    Items = [],
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }

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
                PostReadState.ResolveLabel(post.State),
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
