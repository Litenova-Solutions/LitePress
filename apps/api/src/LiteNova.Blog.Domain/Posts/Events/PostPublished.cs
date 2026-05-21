using LiteNova.Blog.Domain.Authors;
using LiteNova.Blog.Domain.Shared;

namespace LiteNova.Blog.Domain.Posts.Events;

public sealed record PostPublished(
    PostId PostId,
    AuthorId AuthorId,
    DateTimeOffset PublishedAt
) : IDomainEvent;
