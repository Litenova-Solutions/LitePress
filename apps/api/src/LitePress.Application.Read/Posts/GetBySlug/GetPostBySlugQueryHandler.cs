using LitePress.Application.Read.Contracts.Posts.GetPostById;
using LitePress.Application.Read.Contracts.Posts.GetPostBySlug;
using LitePress.Domain.Posts.Exceptions;

namespace LitePress.Application.Read.Posts.GetBySlug;

internal sealed class GetPostBySlugQueryHandler : IQueryHandler<GetPostBySlugQuery, PostDetailResult>
{
    private readonly IDatabaseContext _db;
    public GetPostBySlugQueryHandler(IDatabaseContext db) { _db = db; }

    public async Task<PostDetailResult> HandleAsync(GetPostBySlugQuery query, CancellationToken cancellationToken)
    {
        var post = await PostStateQuery.WherePublished(_db.Posts.AsNoTracking())
            .Where(p => p.Slug.Value == query.Slug)
            .Select(p => new
            {
                p.Id,
                Title = p.Title.Value,
                Slug = p.Slug.Value,
                Content = p.Content.Value,
                Excerpt = p.Excerpt != null ? p.Excerpt.Value : null,
                CoverImageUrl = p.CoverImageUrl != null ? p.CoverImageUrl.Value : null,
                p.AuthorId,
                PublishedAt = EF.Property<DateTimeOffset>(p, PostStateColumns.PublishedAt),
                p.CreatedAt,
                Tags = p.Tags.Select(t => t.TagId).ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (post is null)
        {
            throw new PostNotFoundException(new PostId(Guid.Empty));
        }

        var authorName = await _db.Authors
            .AsNoTracking()
            .Where(a => a.Id == post.AuthorId)
            .Select(a => a.DisplayName)
            .FirstOrDefaultAsync(cancellationToken) ?? string.Empty;

        var tagIds = post.Tags;
        var tags = await _db.Tags
            .AsNoTracking()
            .Where(t => tagIds.Contains(t.Id))
            .Select(t => new TagSummaryResult(t.Id.Value, t.Name.Value, t.Slug.Value))
            .ToListAsync(cancellationToken);

        return new PostDetailResult(
            post.Id.Value,
            post.Title,
            post.Slug,
            post.Content,
            post.Excerpt,
            post.CoverImageUrl,
            authorName,
            "Published",
            post.CreatedAt,
            post.PublishedAt,
            tags);
    }
}
