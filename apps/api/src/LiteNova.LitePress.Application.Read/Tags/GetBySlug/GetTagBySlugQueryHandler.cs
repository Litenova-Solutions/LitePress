using LiteNova.LitePress.Application.Read.Contracts.Tags.GetAllTags;
using LiteNova.LitePress.Application.Read.Contracts.Tags.GetTagBySlug;
using LiteNova.LitePress.Domain.Tags.Exceptions;

namespace LiteNova.LitePress.Application.Read.Tags.GetBySlug;

internal sealed class GetTagBySlugQueryHandler : IQueryHandler<GetTagBySlugQuery, TagResult>
{
    private readonly IDatabaseContext _db;
    public GetTagBySlugQueryHandler(IDatabaseContext db) { _db = db; }

    public async Task<TagResult> HandleAsync(GetTagBySlugQuery query, CancellationToken cancellationToken)
    {
        var tag = await _db.Tags
            .AsNoTracking()
            .Where(t => t.Slug.Value == query.Slug)
            .Select(t => new TagResult(
                t.Id.Value, t.Name.Value, t.Slug.Value,
                _db.Posts.Count(p => p.Tags.Any(pt => pt.TagId == t.Id) && EF.Property<string>(p, "_stateType") == "Published")
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (tag is null)
        {
            throw new TagNotFoundException(new TagId(Guid.Empty));
        }

        return tag;
    }
}
