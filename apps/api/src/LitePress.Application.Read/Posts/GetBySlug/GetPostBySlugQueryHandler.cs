using LitePress.Application.Read.Contracts.Posts.GetPostById;
using LitePress.Application.Read.Contracts.Posts.GetPostBySlug;
using LitePress.Domain.Posts.Exceptions;

namespace LitePress.Application.Read.Posts.GetBySlug;

internal sealed class GetPostBySlugQueryHandler : IQueryHandler<GetPostBySlugQuery, PostDetailResult>
{
    private readonly IReadDatabase _db;
    public GetPostBySlugQueryHandler(IReadDatabase db) { _db = db; }

    public Task<PostDetailResult> HandleAsync(GetPostBySlugQuery query, CancellationToken cancellationToken) =>
        _db.QueryAsync(async (ctx, ct) =>
        {
            var matches = await ctx.ToListAsync(
                ctx.Posts.Where(candidate => candidate.Slug.Value == query.Slug),
                ct);

            var post = matches.FirstOrDefault(candidate => candidate.State is PublishedPostState);

            if (post is null)
            {
                throw new PostNotFoundException(new PostId(Guid.Empty));
            }

            var authors = await PostReadSupport.LoadAuthorNamesAsync(ctx, [post.AuthorId.Value], ct);
            var tags = await PostReadSupport.LoadTagSummariesAsync(
                ctx,
                post.Tags.Select(tag => tag.TagId.Value).ToList(),
                ct);

            return new PostDetailResult(
                post.Id.Value,
                post.Title.Value,
                post.Slug.Value,
                post.Content.Value,
                post.Excerpt?.Value,
                post.CoverImageUrl?.Value,
                authors.GetValueOrDefault(post.AuthorId.Value, string.Empty),
                "Published",
                post.CreatedAt,
                PostStateQuery.GetPublishedAt(post.State),
                post.Tags.Select(tag => tags.GetValueOrDefault(
                    tag.TagId.Value,
                    new TagSummaryResult(tag.TagId.Value, string.Empty, string.Empty))).ToList());
        }, cancellationToken);
}
