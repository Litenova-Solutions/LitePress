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
        var projected = await source.OrderByDescending(p => p.PublishedAt).Skip((query.Page - 1) * query.PageSize).Take(query.PageSize)
            .Select(p => new
            {
                p.Id,
                p.Title,
                p.Slug,
                p.Excerpt,
                p.CoverImageUrl,
                Status = p.Status.ToString(),
                p.PublishedAt,
                p.ReadingTimeMinutes,
                TagIds = p.Tags.Select(t => t.TagId).ToArray()
            })
            .ToListAsync(cancellationToken);

        var allTagIds = projected.SelectMany(p => p.TagIds).Distinct().ToArray();
        var tagLookup = await dbContext.Tags.AsNoTracking()
            .Where(t => allTagIds.Contains(t.Id))
            .ToDictionaryAsync(t => t.Id, t => t.Name, cancellationToken);

        var items = projected
            .Select(p => new PostSummaryItem(
                p.Id,
                p.Title,
                p.Slug,
                p.Excerpt,
                p.CoverImageUrl,
                p.Status,
                p.PublishedAt,
                p.ReadingTimeMinutes,
                p.TagIds.Where(tagLookup.ContainsKey).Select(id => tagLookup[id]).ToArray()))
            .ToList();

        return new GetPublishedPostsQueryResult(items, total);
    }
}
