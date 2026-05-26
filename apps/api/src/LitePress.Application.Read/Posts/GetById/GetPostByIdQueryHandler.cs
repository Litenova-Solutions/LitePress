using LitePress.Application.Read.Contracts.Posts.GetPostById;
using LitePress.Domain.Posts.Exceptions;

namespace LitePress.Application.Read.Posts.GetById;

internal sealed class GetPostByIdQueryHandler : IQueryHandler<GetPostByIdQuery, PostDetailResult>
{
    private readonly IDatabaseContext _db;
    public GetPostByIdQueryHandler(IDatabaseContext db) { _db = db; }

    public async Task<PostDetailResult> HandleAsync(GetPostByIdQuery query, CancellationToken cancellationToken)
    {
        var post = await _db.Posts
            .AsNoTracking()
            .Where(p => p.Id == query.PostId)
            .Select(p => new
            {
                p.Id,
                Title = p.Title.Value,
                Slug = p.Slug.Value,
                Content = p.Content.Value,
                Excerpt = p.Excerpt != null ? p.Excerpt.Value : null,
                CoverImageUrl = p.CoverImageUrl != null ? p.CoverImageUrl.Value : null,
                p.AuthorId,
                StateType = EF.Property<string>(p, PostStateColumns.StateType),
                PublishedAt = EF.Property<DateTimeOffset?>(p, PostStateColumns.PublishedAt),
                ArchivedAt = EF.Property<DateTimeOffset?>(p, PostStateColumns.ArchivedAt),
                p.CreatedAt,
                Tags = p.Tags.Select(t => t.TagId).ToList()
            })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new PostNotFoundException(query.PostId);

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

        var state = PostStateQuery.FromColumns(post.StateType, post.PublishedAt, post.ArchivedAt);

        return new PostDetailResult(
            post.Id.Value,
            post.Title,
            post.Slug,
            post.Content,
            post.Excerpt,
            post.CoverImageUrl,
            authorName,
            PostReadState.ResolveLabel(state),
            post.CreatedAt,
            state is PublishedPostState published ? published.PublishedAt : null,
            tags);
    }
}
