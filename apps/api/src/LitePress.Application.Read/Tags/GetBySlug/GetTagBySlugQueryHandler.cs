using LitePress.Application.Read.Contracts.Tags.GetAllTags;
using LitePress.Application.Read.Contracts.Tags.GetTagBySlug;
using LitePress.Domain.Tags.Exceptions;

namespace LitePress.Application.Read.Tags.GetBySlug;

internal sealed class GetTagBySlugQueryHandler : IQueryHandler<GetTagBySlugQuery, TagResult>
{
    private readonly IReadDatabase _db;
    public GetTagBySlugQueryHandler(IReadDatabase db) { _db = db; }

    public Task<TagResult> HandleAsync(GetTagBySlugQuery query, CancellationToken cancellationToken) =>
        _db.QueryAsync(async (ctx, ct) =>
        {
            var tag = await ctx.FirstOrDefaultAsync(
                ctx.Tags.Where(candidate => candidate.Slug.Value == query.Slug),
                ct);

            if (tag is null)
            {
                throw new TagNotFoundException(new TagId(Guid.Empty));
            }

            var count = (await ctx.ToListAsync(ctx.Posts, ct))
                .Count(post =>
                    post.State is PublishedPostState
                    && post.Tags.Any(postTag => postTag.TagId.Value == tag.Id.Value));

            return new TagResult(tag.Id.Value, tag.Name.Value, tag.Slug.Value, count);
        }, cancellationToken);
}
