using LiteNova.Blog.Domain.Authors;
using LiteNova.Blog.Domain.Shared;
using LiteNova.Blog.Domain.Tags;

namespace LiteNova.Blog.Domain.Posts.Events;

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
