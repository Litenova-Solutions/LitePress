using LiteBus.CQRS;
using LiteNova.Blog.Application.Common.Exceptions;
using LiteNova.Blog.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LiteNova.Blog.Application.Posts.Queries.GetPostBySlug;

public sealed class GetPostBySlugQueryHandler(IBlogDbContext dbContext) : IQueryHandler<GetPostBySlugQuery, GetPostBySlugQueryResult>
{
    public async Task<GetPostBySlugQueryResult> HandleAsync(GetPostBySlugQuery query, CancellationToken cancellationToken)
    {
        var post = await dbContext.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.Slug == query.Slug, cancellationToken) ?? throw new PostNotFoundException(Guid.Empty);
        return new GetPostBySlugQueryResult(post.Id, post.Title, post.Slug, post.Excerpt, post.CoverImageUrl, post.Status.ToString(), post.PublishedAt, post.ReadingTimeMinutes, post.Tags.Select(t => t.TagId.ToString()).ToArray(), post.Body);
    }
}
