using LiteNova.LitePress.Domain.Authors;
using LiteNova.LitePress.Domain.Shared;
using LiteNova.LitePress.Domain.Tags;

namespace LiteNova.LitePress.Domain.Posts.Events;

public sealed record PostCreated(
    PostId PostId,
    AuthorId AuthorId,
    PostTitle Title,
    PostSlug Slug,
    PostContent Content,
    PostExcerpt? Excerpt,
    PostCoverImageUrl? CoverImageUrl,
    IReadOnlyList<TagId> TagIds
) : IDomainEvent;
