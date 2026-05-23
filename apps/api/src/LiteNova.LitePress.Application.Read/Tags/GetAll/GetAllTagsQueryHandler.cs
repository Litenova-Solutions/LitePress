using LiteNova.LitePress.Application.Read.Contracts.Tags.GetAllTags;

namespace LiteNova.LitePress.Application.Read.Tags.GetAll;

internal sealed class GetAllTagsQueryHandler : IQueryHandler<GetAllTagsQuery, IReadOnlyList<TagResult>>
{
    private readonly IDatabaseContext _db;
    public GetAllTagsQueryHandler(IDatabaseContext db) { _db = db; }

    public async Task<IReadOnlyList<TagResult>> HandleAsync(GetAllTagsQuery query, CancellationToken cancellationToken)
    {
        return await _db.Tags
            .AsNoTracking()
            .OrderBy(t => t.Name.Value)
            .Select(t => new TagResult(
                t.Id.Value,
                t.Name.Value,
                t.Slug.Value,
                _db.Posts.Count(p => p.Tags.Any(pt => pt.TagId == t.Id) && EF.Property<string>(p, "_stateType") == "Published")
            ))
            .ToListAsync(cancellationToken);
    }
}
