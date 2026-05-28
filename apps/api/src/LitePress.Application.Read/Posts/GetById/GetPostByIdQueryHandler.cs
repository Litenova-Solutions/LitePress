using LitePress.Application.Read.Contracts.Posts.GetPostById;
using LitePress.Domain.Posts.Exceptions;

namespace LitePress.Application.Read.Posts.GetById;

internal sealed class GetPostByIdQueryHandler : IQueryHandler<GetPostByIdQuery, PostDetailResult>
{
    private readonly IReadDatabase _db;
    public GetPostByIdQueryHandler(IReadDatabase db) { _db = db; }

    public Task<PostDetailResult> HandleAsync(GetPostByIdQuery query, CancellationToken cancellationToken) =>
        _db.QueryAsync(async (ctx, ct) =>
        {
            var post = await ctx.LoadAsync<Post>(query.PostId, ct)
                ?? throw new PostNotFoundException(query.PostId);

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
                PostReadState.ResolveLabel(post.State),
                post.CreatedAt,
                PostStateQuery.GetPublishedAt(post.State),
                post.Tags.Select(tag => tags.GetValueOrDefault(
                    tag.TagId.Value,
                    new TagSummaryResult(tag.TagId.Value, string.Empty, string.Empty))).ToList());
        }, cancellationToken);
}
