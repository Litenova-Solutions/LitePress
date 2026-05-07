using LiteBus.CQRS;
using LiteNova.Blog.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LiteNova.Blog.Application.Posts.Queries.GetAllPosts;

public sealed class GetAllPostsQueryHandler(IBlogDbContext dbContext) : IQueryHandler<GetAllPostsQuery, GetAllPostsQueryResult>
{
    public async Task<GetAllPostsQueryResult> HandleAsync(GetAllPostsQuery query, CancellationToken cancellationToken)
    {
        var source = dbContext.Posts.AsNoTracking();
        var total = await source.CountAsync(cancellationToken);
        var items = await source.OrderByDescending(p => p.UpdatedAt).Skip((query.Page - 1) * query.PageSize).Take(query.PageSize)
            .Select(p => new PostSummaryItem(p.Id, p.Title, p.Slug, p.Excerpt, p.CoverImageUrl, p.Status.ToString(), p.PublishedAt, p.ReadingTimeMinutes, Array.Empty<string>()))
            .ToListAsync(cancellationToken);
        return new GetAllPostsQueryResult(items, total);
    }
}
