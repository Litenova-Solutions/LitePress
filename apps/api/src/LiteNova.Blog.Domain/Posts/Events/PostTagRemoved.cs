using LiteNova.Blog.Domain.Shared;
using LiteNova.Blog.Domain.Tags;

namespace LiteNova.Blog.Domain.Posts.Events;

public sealed record PostTagRemoved(PostId PostId, TagId TagId) : IDomainEvent;
