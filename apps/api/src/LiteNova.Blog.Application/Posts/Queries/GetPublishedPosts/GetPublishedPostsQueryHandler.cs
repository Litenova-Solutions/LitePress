using LiteBus.CQRS;
using LiteNova.Blog.Application.Common.Interfaces;
using LiteNova.Blog.Domain.Posts;
using Microsoft.EntityFrameworkCore;

namespace LiteNova.Blog.Application.Posts.Queries.GetPublishedPosts;

public sealed class GetPublishedPostsQueryHandler(IBlogDbContext dbContext) : IQueryHandler<GetPublishedPostsQuery, GetPublishedPostsQueryResult>
{
    public async Task<GetPublishedPostsQueryResult> HandleAsync(GetPublishedPostsQuery query, CancellationToken cancellationToken)
    {
        var source = dbContext.Posts.AsNoTracking().Where(p => p.Status == PostStatus.Published);
        var total = await source.CountAsync(cancellationToken);
        var items = await source.OrderByDescending(p => p.PublishedAt).Skip((query.Page - 1) * query.PageSize).Take(query.PageSize)
            .Select(p => new PostSummaryItem(p.Id, p.Title, p.Slug, p.Excerpt, p.CoverImageUrl, p.Status.ToString(), p.PublishedAt, p.ReadingTimeMinutes, Array.Empty<string>()))
            .ToListAsync(cancellationToken);
        return new GetPublishedPostsQueryResult(items, total);
    }
}
