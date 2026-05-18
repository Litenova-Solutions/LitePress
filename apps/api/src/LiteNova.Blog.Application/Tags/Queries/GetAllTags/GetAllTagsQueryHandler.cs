using LiteBus.CQRS;
using LiteNova.Blog.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LiteNova.Blog.Application.Tags.Queries.GetAllTags;

public sealed class GetAllTagsQueryHandler(IBlogDbContext dbContext) : IQueryHandler<GetAllTagsQuery, IReadOnlyCollection<GetAllTagsQueryResult>>
{
    public async Task<IReadOnlyCollection<GetAllTagsQueryResult>> HandleAsync(GetAllTagsQuery query, CancellationToken cancellationToken)
        => await dbContext.Tags.AsNoTracking().OrderBy(t => t.Name).Select(t => new GetAllTagsQueryResult(t.Id, t.Name, t.Slug)).ToListAsync(cancellationToken);
}
