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
        var tagIds = post.Tags.Select(t => t.TagId).Distinct().ToArray();
        var tagLookup = await dbContext.Tags.AsNoTracking()
            .Where(t => tagIds.Contains(t.Id))
            .ToDictionaryAsync(t => t.Id, t => t.Name, cancellationToken);

        return new GetPostBySlugQueryResult(
            post.Id,
            post.Title,
            post.Slug,
            post.Excerpt,
            post.CoverImageUrl,
            post.Status.ToString(),
            post.PublishedAt,
            post.ReadingTimeMinutes,
            tagIds.Where(tagLookup.ContainsKey).Select(id => tagLookup[id]).ToArray(),
            post.Body);
    }
}
