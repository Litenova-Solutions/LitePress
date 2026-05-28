using LitePress.Application.Read.Contracts.Tags.GetAllTags;

namespace LitePress.Application.Read.Tags.GetAll;

internal sealed class GetAllTagsQueryHandler : IQueryHandler<GetAllTagsQuery, IReadOnlyList<TagResult>>
{
    private readonly IReadDatabase _db;
    public GetAllTagsQueryHandler(IReadDatabase db) { _db = db; }

    public Task<IReadOnlyList<TagResult>> HandleAsync(GetAllTagsQuery query, CancellationToken cancellationToken) =>
        _db.QueryAsync(async (ctx, ct) =>
        {
            var tags = await ctx.ToListAsync(ctx.Tags, ct);

            var publishedPosts = (await ctx.ToListAsync(ctx.Posts, ct))
                .Where(post => post.State is PublishedPostState);

            var counts = new Dictionary<Guid, int>();
            foreach (var post in publishedPosts)
            {
                foreach (var postTag in post.Tags)
                {
                    counts.TryGetValue(postTag.TagId.Value, out var current);
                    counts[postTag.TagId.Value] = current + 1;
                }
            }

            IReadOnlyList<TagResult> results = tags
                .OrderBy(tag => tag.Name.Value)
                .Select(tag => new TagResult(
                    tag.Id.Value,
                    tag.Name.Value,
                    tag.Slug.Value,
                    counts.GetValueOrDefault(tag.Id.Value, 0)))
                .ToList();

            return results;
        }, cancellationToken);
}
