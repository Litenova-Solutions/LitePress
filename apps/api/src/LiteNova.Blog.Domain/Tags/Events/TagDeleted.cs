using LiteNova.Blog.Domain.Shared;

namespace LiteNova.Blog.Domain.Tags.Events;

public sealed record TagDeleted(TagId TagId) : IDomainEvent;
