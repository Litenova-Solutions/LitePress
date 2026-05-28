using LitePress.Application.Read.Contracts.Posts.GetAllPosts;
using LitePress.Application.Read.Contracts.Posts.GetPostsByTag;

namespace LitePress.Application.Read.Posts.GetByTag;

internal sealed class GetPostsByTagQueryHandler : IQueryHandler<GetPostsByTagQuery, PagedResult<PostSummaryResult>>
{
    private readonly IReadDatabase _db;
    public GetPostsByTagQueryHandler(IReadDatabase db) { _db = db; }

    public Task<PagedResult<PostSummaryResult>> HandleAsync(GetPostsByTagQuery query, CancellationToken cancellationToken) =>
        _db.QueryAsync(async (ctx, ct) =>
        {
            var pageSize = Math.Min(query.Pagination.PageSize, PaginationParameters.MaxPageSize);
            var pageNumber = query.Pagination.PageNumber;
            var start = (pageNumber - 1) * pageSize;

            var tag = await ctx.FirstOrDefaultAsync(
                ctx.Tags.Where(candidate => candidate.Slug.Value == query.TagSlug),
                ct);

            if (tag is null)
            {
                return new PagedResult<PostSummaryResult>
                {
                    Items = [],
                    TotalCount = 0,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }

            var publishedWithTag = (await ctx.ToListAsync(ctx.Posts, ct))
                .Where(post =>
                    post.State is PublishedPostState
                    && post.Tags.Any(postTag => postTag.TagId.Value == tag.Id.Value))
                .OrderByDescending(post => ((PublishedPostState)post.State).PublishedAt)
                .ToList();

            var totalCount = query.Pagination.SkipTotalCount ? 0 : publishedWithTag.Count;

            var posts = publishedWithTag
                .Skip(start)
                .Take(pageSize)
                .ToList();

            var authorIds = posts.Select(post => post.AuthorId.Value).Distinct().ToList();
            var authors = await PostReadSupport.LoadAuthorNamesAsync(ctx, authorIds, ct);
            var tagLookup = await PostReadSupport.LoadTagSummariesAsync(
                ctx,
                posts.SelectMany(post => post.Tags.Select(postTag => postTag.TagId.Value)).Distinct().ToList(),
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
                post.Tags.Select(postTag => tagLookup.GetValueOrDefault(
                    postTag.TagId.Value,
                    new TagSummaryResult(postTag.TagId.Value, string.Empty, string.Empty))).ToList()
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
