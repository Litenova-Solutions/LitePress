using LiteNova.Blog.Domain.Shared;

namespace LiteNova.Blog.Domain.Authors.Events;

public sealed record AuthorRegistered(AuthorId AuthorId, string DisplayName) : IDomainEvent;
