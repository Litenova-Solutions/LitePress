using LiteBus.Queries.Abstractions;
using LiteNova.Blog.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LiteNova.Blog.Application.Tags.GetAllTags;

/// <summary>
/// Handles the GetAllTagsQuery use case.
/// </summary>
public sealed class GetAllTagsQueryHandler(IBlogDbContext dbContext) : IQueryHandler<GetAllTagsQuery, IReadOnlyCollection<GetAllTagsQueryResult>>
{
    public async Task<IReadOnlyCollection<GetAllTagsQueryResult>> HandleAsync(GetAllTagsQuery query, CancellationToken cancellationToken)
        => await dbContext.Tags.AsNoTracking().OrderBy(t => t.Name).Select(t => new GetAllTagsQueryResult(t.Id, t.Name, t.Slug)).ToListAsync(cancellationToken);
}
