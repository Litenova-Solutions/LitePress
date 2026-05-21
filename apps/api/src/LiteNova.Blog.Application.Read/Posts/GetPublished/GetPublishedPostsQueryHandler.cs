using LiteNova.Blog.Application.Read.Contracts.Posts.GetAllPosts;
using LiteNova.Blog.Application.Read.Contracts.Posts.GetPublishedPosts;

namespace LiteNova.Blog.Application.Read.Posts.GetPublished;

internal sealed class GetPublishedPostsQueryHandler : IQueryHandler<GetPublishedPostsQuery, PagedResult<PostSummaryResult>>
{
    private readonly IDatabaseContext _db;
    public GetPublishedPostsQueryHandler(IDatabaseContext db) { _db = db; }

    public async Task<PagedResult<PostSummaryResult>> HandleAsync(GetPublishedPostsQuery query, CancellationToken cancellationToken)
    {
        var pageSize = Math.Min(query.Pagination.PageSize, PaginationParameters.MaxPageSize);
        var pageNumber = query.Pagination.PageNumber;

        var baseQuery = _db.Posts.AsNoTracking()
            .Where(p => EF.Property<string>(p, "_stateType") == "Published");

        var totalCount = query.Pagination.SkipTotalCount ? 0 : await baseQuery.CountAsync(cancellationToken);

        var posts = await baseQuery
            .OrderByDescending(p => p.PublishedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new
            {
                p.Id, Title = p.Title.Value, Slug = p.Slug.Value,
                Excerpt = p.Excerpt != null ? p.Excerpt.Value : null,
                CoverImageUrl = p.CoverImageUrl != null ? p.CoverImageUrl.Value : null,
                p.AuthorId, p.PublishedAt, p.CreatedAt,
                Tags = p.Tags.Select(t => t.TagId).ToList()
            })
            .ToListAsync(cancellationToken);

        var authorIds = posts.Select(p => p.AuthorId).Distinct().ToList();
        var authors = await _db.Authors.AsNoTracking()
            .Where(a => authorIds.Contains(a.Id))
            .Select(a => new { a.Id, a.DisplayName })
            .ToDictionaryAsync(a => a.Id, a => a.DisplayName, cancellationToken);

        var allTagIds = posts.SelectMany(p => p.Tags).Distinct().ToList();
        var tagLookup = await _db.Tags.AsNoTracking()
            .Where(t => allTagIds.Contains(t.Id))
            .Select(t => new TagSummaryResult(t.Id.Value, t.Name.Value, t.Slug.Value))
            .ToDictionaryAsync(t => new TagId(t.TagId), cancellationToken);

        var items = posts.Select(p => new PostSummaryResult(
            p.Id.Value, p.Title, p.Slug, p.Excerpt, p.CoverImageUrl,
            authors.GetValueOrDefault(p.AuthorId, string.Empty),
            "Published", p.CreatedAt, p.PublishedAt,
            p.Tags.Select(id => tagLookup.GetValueOrDefault(id, new TagSummaryResult(id.Value, string.Empty, string.Empty))).ToList()
        )).ToList();

        return new PagedResult<PostSummaryResult>
        {
            Items = items, TotalCount = totalCount,
            PageNumber = pageNumber, PageSize = pageSize
        };
    }
}
