using LiteNova.Blog.Domain.Shared;

namespace LiteNova.Blog.Domain.Posts.Events;

public sealed record PostDeleted(PostId PostId) : IDomainEvent;
